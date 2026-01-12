using DOSFinal.Application.DTOs;

namespace DOSFinal.Tests.DTOs;

public class ReservationDtoTests
{
    [Fact]
    public void CreateReservationDto_DefaultValues_AreCorrect()
    {
        // Arrange & Act
        var dto = new CreateReservationDto();

        // Assert
        Assert.Equal(string.Empty, dto.CustomerName);
        Assert.Equal(0, dto.TableNumber);
        Assert.Equal(0, dto.NumberOfPeople);
    }

    [Fact]
    public void CreateReservationDto_CanSetAllProperties()
    {
        // Arrange & Act
        var dto = new CreateReservationDto
        {
            CustomerName = "John Doe",
            ReservationDate = new DateOnly(2026, 1, 20),
            ReservationTime = new TimeOnly(19, 30),
            TableNumber = 5,
            NumberOfPeople = 4
        };

        // Assert
        Assert.Equal("John Doe", dto.CustomerName);
        Assert.Equal(new DateOnly(2026, 1, 20), dto.ReservationDate);
        Assert.Equal(new TimeOnly(19, 30), dto.ReservationTime);
        Assert.Equal(5, dto.TableNumber);
        Assert.Equal(4, dto.NumberOfPeople);
    }

    [Fact]
    public void UpdateReservationDto_CanSetAllProperties()
    {
        // Arrange & Act
        var dto = new UpdateReservationDto
        {
            CustomerName = "Jane Doe",
            ReservationDate = new DateOnly(2026, 2, 15),
            ReservationTime = new TimeOnly(20, 0),
            TableNumber = 3,
            NumberOfPeople = 2
        };

        // Assert
        Assert.Equal("Jane Doe", dto.CustomerName);
        Assert.Equal(new DateOnly(2026, 2, 15), dto.ReservationDate);
        Assert.Equal(new TimeOnly(20, 0), dto.ReservationTime);
        Assert.Equal(3, dto.TableNumber);
        Assert.Equal(2, dto.NumberOfPeople);
    }

    [Fact]
    public void ReservationDto_CanSetAllProperties()
    {
        // Arrange & Act
        var createdAt = DateTime.UtcNow;
        var dto = new ReservationDto
        {
            Id = 1,
            CustomerName = "John Doe",
            ReservationDate = new DateOnly(2026, 1, 20),
            ReservationTime = new TimeOnly(19, 30),
            TableNumber = 5,
            NumberOfPeople = 4,
            CreatedAt = createdAt
        };

        // Assert
        Assert.Equal(1, dto.Id);
        Assert.Equal("John Doe", dto.CustomerName);
        Assert.Equal(new DateOnly(2026, 1, 20), dto.ReservationDate);
        Assert.Equal(new TimeOnly(19, 30), dto.ReservationTime);
        Assert.Equal(5, dto.TableNumber);
        Assert.Equal(4, dto.NumberOfPeople);
        Assert.Equal(createdAt, dto.CreatedAt);
    }
}
