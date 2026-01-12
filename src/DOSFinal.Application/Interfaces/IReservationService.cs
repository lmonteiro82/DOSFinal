using DOSFinal.Application.DTOs;

namespace DOSFinal.Application.Interfaces;

public interface IReservationService
{
    Task<ReservationDto?> GetByIdAsync(int id);
    Task<IEnumerable<ReservationDto>> GetAllAsync();
    Task<IEnumerable<ReservationDto>> GetByDateAsync(DateOnly date);
    Task<ReservationDto> CreateAsync(CreateReservationDto dto);
    Task<ReservationDto?> UpdateAsync(int id, UpdateReservationDto dto);
    Task<bool> DeleteAsync(int id);
}
