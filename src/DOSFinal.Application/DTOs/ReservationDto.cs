namespace DOSFinal.Application.DTOs;

public class ReservationDto
{
    public int Id { get; set; }
    public string CustomerName { get; set; } = string.Empty;
    public DateOnly ReservationDate { get; set; }
    public TimeOnly ReservationTime { get; set; }
    public int TableNumber { get; set; }
    public int NumberOfPeople { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class CreateReservationDto
{
    public string CustomerName { get; set; } = string.Empty;
    public DateOnly ReservationDate { get; set; }
    public TimeOnly ReservationTime { get; set; }
    public int TableNumber { get; set; }
    public int NumberOfPeople { get; set; }
}

public class UpdateReservationDto
{
    public string CustomerName { get; set; } = string.Empty;
    public DateOnly ReservationDate { get; set; }
    public TimeOnly ReservationTime { get; set; }
    public int TableNumber { get; set; }
    public int NumberOfPeople { get; set; }
}
