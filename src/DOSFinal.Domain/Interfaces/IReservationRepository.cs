using DOSFinal.Domain.Entities;

namespace DOSFinal.Domain.Interfaces;

public interface IReservationRepository
{
    Task<Reservation?> GetByIdAsync(int id);
    Task<IEnumerable<Reservation>> GetAllAsync();
    Task<IEnumerable<Reservation>> GetByDateAsync(DateOnly date);
    Task<Reservation> CreateAsync(Reservation reservation);
    Task<Reservation?> UpdateAsync(Reservation reservation);
    Task<bool> DeleteAsync(int id);
    Task<bool> HasConflictAsync(DateOnly date, TimeOnly time, int tableNumber, int? excludeReservationId = null);
}
