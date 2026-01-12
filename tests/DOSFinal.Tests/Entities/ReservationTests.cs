using DOSFinal.Domain.Entities;

namespace DOSFinal.Tests.Entities;

public class ReservationTests
{
    [Fact]
    public void Reservation_DefaultValues_AreCorrect()
    {
        // Arrange & Act
        var reservation = new Reservation();

        // Assert
        Assert.Equal(0, reservation.Id);
        Assert.Equal(string.Empty, reservation.CustomerName);
        Assert.Equal(0, reservation.TableNumber);
        Assert.Equal(0, reservation.NumberOfPeople);
        Assert.NotEqual(default, reservation.CreatedAt);
    }

    [Fact]
    public void Reservation_CanSetAllProperties()
    {
        // Arrange
        var reservation = new Reservation
        {
            Id = 1,
            CustomerName = "John Doe",
            ReservationDate = new DateOnly(2026, 1, 20),
            ReservationTime = new TimeOnly(19, 30),
            TableNumber = 5,
            NumberOfPeople = 4
        };

        // Assert
        Assert.Equal(1, reservation.Id);
        Assert.Equal("John Doe", reservation.CustomerName);
        Assert.Equal(new DateOnly(2026, 1, 20), reservation.ReservationDate);
        Assert.Equal(new TimeOnly(19, 30), reservation.ReservationTime);
        Assert.Equal(5, reservation.TableNumber);
        Assert.Equal(4, reservation.NumberOfPeople);
    }

    [Theory]
    [InlineData("Alice", 1, 2)]
    [InlineData("Bob", 10, 8)]
    [InlineData("Charlie", 5, 1)]
    public void Reservation_MultipleValidConfigurations_CreatesSuccessfully(
        string customerName, int tableNumber, int numberOfPeople)
    {
        // Arrange & Act
        var reservation = new Reservation
        {
            CustomerName = customerName,
            TableNumber = tableNumber,
            NumberOfPeople = numberOfPeople,
            ReservationDate = DateOnly.FromDateTime(DateTime.Today),
            ReservationTime = new TimeOnly(18, 0)
        };

        // Assert
        Assert.Equal(customerName, reservation.CustomerName);
        Assert.Equal(tableNumber, reservation.TableNumber);
        Assert.Equal(numberOfPeople, reservation.NumberOfPeople);
    }
}
