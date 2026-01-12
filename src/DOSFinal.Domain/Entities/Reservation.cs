namespace DOSFinal.Domain.Entities;

public class Reservation
{
    public int Id { get; set; }
    public string CustomerName { get; set; } = string.Empty;
    public DateOnly ReservationDate { get; set; }
    public TimeOnly ReservationTime { get; set; }
    public int TableNumber { get; set; }
    public int NumberOfPeople { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
