using DOSFinal.Application.DTOs;
using DOSFinal.Application.Services;
using DOSFinal.Domain.Entities;
using DOSFinal.Domain.Interfaces;
using Moq;

namespace DOSFinal.Tests.Services;

public class ReservationServiceTests
{
    private readonly Mock<IReservationRepository> _mockRepository;
    private readonly ReservationService _service;

    public ReservationServiceTests()
    {
        _mockRepository = new Mock<IReservationRepository>();
        _service = new ReservationService(_mockRepository.Object);
    }

    #region GetByIdAsync Tests

    [Fact]
    public async Task GetByIdAsync_ExistingId_ReturnsReservation()
    {
        // Arrange
        var reservation = CreateTestReservation(1);
        _mockRepository.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(reservation);

        // Act
        var result = await _service.GetByIdAsync(1);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(reservation.Id, result.Id);
        Assert.Equal(reservation.CustomerName, result.CustomerName);
    }

    [Fact]
    public async Task GetByIdAsync_NonExistingId_ReturnsNull()
    {
        // Arrange
        _mockRepository.Setup(r => r.GetByIdAsync(999)).ReturnsAsync((Reservation?)null);

        // Act
        var result = await _service.GetByIdAsync(999);

        // Assert
        Assert.Null(result);
    }

    #endregion

    #region GetAllAsync Tests

    [Fact]
    public async Task GetAllAsync_ReturnsAllReservations()
    {
        // Arrange
        var reservations = new List<Reservation>
        {
            CreateTestReservation(1),
            CreateTestReservation(2),
            CreateTestReservation(3)
        };
        _mockRepository.Setup(r => r.GetAllAsync()).ReturnsAsync(reservations);

        // Act
        var result = await _service.GetAllAsync();

        // Assert
        Assert.Equal(3, result.Count());
    }

    [Fact]
    public async Task GetAllAsync_EmptyDatabase_ReturnsEmptyList()
    {
        // Arrange
        _mockRepository.Setup(r => r.GetAllAsync()).ReturnsAsync(new List<Reservation>());

        // Act
        var result = await _service.GetAllAsync();

        // Assert
        Assert.Empty(result);
    }

    #endregion

    #region GetByDateAsync Tests

    [Fact]
    public async Task GetByDateAsync_ExistingDate_ReturnsReservations()
    {
        // Arrange
        var date = new DateOnly(2026, 1, 15);
        var reservations = new List<Reservation>
        {
            CreateTestReservation(1, date),
            CreateTestReservation(2, date)
        };
        _mockRepository.Setup(r => r.GetByDateAsync(date)).ReturnsAsync(reservations);

        // Act
        var result = await _service.GetByDateAsync(date);

        // Assert
        Assert.Equal(2, result.Count());
    }

    [Fact]
    public async Task GetByDateAsync_NoReservationsOnDate_ReturnsEmptyList()
    {
        // Arrange
        var date = new DateOnly(2026, 12, 25);
        _mockRepository.Setup(r => r.GetByDateAsync(date)).ReturnsAsync(new List<Reservation>());

        // Act
        var result = await _service.GetByDateAsync(date);

        // Assert
        Assert.Empty(result);
    }

    #endregion

    #region CreateAsync Tests

    [Fact]
    public async Task CreateAsync_ValidReservation_ReturnsCreatedReservation()
    {
        // Arrange
        var dto = new CreateReservationDto
        {
            CustomerName = "John Doe",
            ReservationDate = new DateOnly(2026, 1, 20),
            ReservationTime = new TimeOnly(19, 0),
            TableNumber = 5,
            NumberOfPeople = 4
        };

        _mockRepository.Setup(r => r.HasConflictAsync(
            dto.ReservationDate, dto.ReservationTime, dto.TableNumber, null))
            .ReturnsAsync(false);

        _mockRepository.Setup(r => r.CreateAsync(It.IsAny<Reservation>()))
            .ReturnsAsync((Reservation r) => { r.Id = 1; return r; });

        // Act
        var result = await _service.CreateAsync(dto);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(dto.CustomerName, result.CustomerName);
        Assert.Equal(dto.TableNumber, result.TableNumber);
        Assert.Equal(dto.NumberOfPeople, result.NumberOfPeople);
    }

    [Fact]
    public async Task CreateAsync_ConflictingReservation_ThrowsInvalidOperationException()
    {
        // Arrange
        var dto = new CreateReservationDto
        {
            CustomerName = "John Doe",
            ReservationDate = new DateOnly(2026, 1, 20),
            ReservationTime = new TimeOnly(19, 0),
            TableNumber = 5,
            NumberOfPeople = 4
        };

        _mockRepository.Setup(r => r.HasConflictAsync(
            dto.ReservationDate, dto.ReservationTime, dto.TableNumber, null))
            .ReturnsAsync(true);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(
            () => _service.CreateAsync(dto));

        Assert.Contains("already reserved", exception.Message);
    }

    #endregion

    #region UpdateAsync Tests

    [Fact]
    public async Task UpdateAsync_ExistingReservation_ReturnsUpdatedReservation()
    {
        // Arrange
        var existingReservation = CreateTestReservation(1);
        var dto = new UpdateReservationDto
        {
            CustomerName = "Jane Doe",
            ReservationDate = new DateOnly(2026, 2, 10),
            ReservationTime = new TimeOnly(20, 0),
            TableNumber = 3,
            NumberOfPeople = 2
        };

        _mockRepository.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(existingReservation);
        _mockRepository.Setup(r => r.HasConflictAsync(
            dto.ReservationDate, dto.ReservationTime, dto.TableNumber, 1))
            .ReturnsAsync(false);
        _mockRepository.Setup(r => r.UpdateAsync(It.IsAny<Reservation>()))
            .ReturnsAsync((Reservation r) => r);

        // Act
        var result = await _service.UpdateAsync(1, dto);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(dto.CustomerName, result.CustomerName);
        Assert.Equal(dto.TableNumber, result.TableNumber);
    }

    [Fact]
    public async Task UpdateAsync_NonExistingReservation_ReturnsNull()
    {
        // Arrange
        var dto = new UpdateReservationDto
        {
            CustomerName = "Jane Doe",
            ReservationDate = new DateOnly(2026, 2, 10),
            ReservationTime = new TimeOnly(20, 0),
            TableNumber = 3,
            NumberOfPeople = 2
        };

        _mockRepository.Setup(r => r.GetByIdAsync(999)).ReturnsAsync((Reservation?)null);

        // Act
        var result = await _service.UpdateAsync(999, dto);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task UpdateAsync_ConflictingReservation_ThrowsInvalidOperationException()
    {
        // Arrange
        var existingReservation = CreateTestReservation(1);
        var dto = new UpdateReservationDto
        {
            CustomerName = "Jane Doe",
            ReservationDate = new DateOnly(2026, 2, 10),
            ReservationTime = new TimeOnly(20, 0),
            TableNumber = 3,
            NumberOfPeople = 2
        };

        _mockRepository.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(existingReservation);
        _mockRepository.Setup(r => r.HasConflictAsync(
            dto.ReservationDate, dto.ReservationTime, dto.TableNumber, 1))
            .ReturnsAsync(true);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(
            () => _service.UpdateAsync(1, dto));

        Assert.Contains("already reserved", exception.Message);
    }

    #endregion

    #region DeleteAsync Tests

    [Fact]
    public async Task DeleteAsync_ExistingReservation_ReturnsTrue()
    {
        // Arrange
        _mockRepository.Setup(r => r.DeleteAsync(1)).ReturnsAsync(true);

        // Act
        var result = await _service.DeleteAsync(1);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public async Task DeleteAsync_NonExistingReservation_ReturnsFalse()
    {
        // Arrange
        _mockRepository.Setup(r => r.DeleteAsync(999)).ReturnsAsync(false);

        // Act
        var result = await _service.DeleteAsync(999);

        // Assert
        Assert.False(result);
    }

    #endregion

    #region Helper Methods

    private static Reservation CreateTestReservation(int id, DateOnly? date = null)
    {
        return new Reservation
        {
            Id = id,
            CustomerName = $"Customer {id}",
            ReservationDate = date ?? new DateOnly(2026, 1, 15),
            ReservationTime = new TimeOnly(19, 0),
            TableNumber = id,
            NumberOfPeople = 4,
            CreatedAt = DateTime.UtcNow
        };
    }

    #endregion
}
