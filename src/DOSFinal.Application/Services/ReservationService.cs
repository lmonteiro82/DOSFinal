using DOSFinal.Application.DTOs;
using DOSFinal.Application.Interfaces;
using DOSFinal.Domain.Entities;
using DOSFinal.Domain.Interfaces;

namespace DOSFinal.Application.Services;

public class ReservationService : IReservationService
{
    private readonly IReservationRepository _repository;

    public ReservationService(IReservationRepository repository)
    {
        _repository = repository;
    }

    public async Task<ReservationDto?> GetByIdAsync(int id)
    {
        var reservation = await _repository.GetByIdAsync(id);
        return reservation == null ? null : MapToDto(reservation);
    }

    public async Task<IEnumerable<ReservationDto>> GetAllAsync()
    {
        var reservations = await _repository.GetAllAsync();
        return reservations.Select(MapToDto);
    }

    public async Task<IEnumerable<ReservationDto>> GetByDateAsync(DateOnly date)
    {
        var reservations = await _repository.GetByDateAsync(date);
        return reservations.Select(MapToDto);
    }

    public async Task<ReservationDto> CreateAsync(CreateReservationDto dto)
    {
        // Validate for conflicts
        var hasConflict = await _repository.HasConflictAsync(
            dto.ReservationDate,
            dto.ReservationTime,
            dto.TableNumber);

        if (hasConflict)
        {
            throw new InvalidOperationException(
                $"Table {dto.TableNumber} is already reserved at {dto.ReservationTime} on {dto.ReservationDate}");
        }

        var reservation = new Reservation
        {
            CustomerName = dto.CustomerName,
            ReservationDate = dto.ReservationDate,
            ReservationTime = dto.ReservationTime,
            TableNumber = dto.TableNumber,
            NumberOfPeople = dto.NumberOfPeople,
            CreatedAt = DateTime.UtcNow
        };

        var created = await _repository.CreateAsync(reservation);
        return MapToDto(created);
    }

    public async Task<ReservationDto?> UpdateAsync(int id, UpdateReservationDto dto)
    {
        var existing = await _repository.GetByIdAsync(id);
        if (existing == null)
        {
            return null;
        }

        // Validate for conflicts (excluding current reservation)
        var hasConflict = await _repository.HasConflictAsync(
            dto.ReservationDate,
            dto.ReservationTime,
            dto.TableNumber,
            id);

        if (hasConflict)
        {
            throw new InvalidOperationException(
                $"Table {dto.TableNumber} is already reserved at {dto.ReservationTime} on {dto.ReservationDate}");
        }

        existing.CustomerName = dto.CustomerName;
        existing.ReservationDate = dto.ReservationDate;
        existing.ReservationTime = dto.ReservationTime;
        existing.TableNumber = dto.TableNumber;
        existing.NumberOfPeople = dto.NumberOfPeople;

        var updated = await _repository.UpdateAsync(existing);
        return updated == null ? null : MapToDto(updated);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        return await _repository.DeleteAsync(id);
    }

    private static ReservationDto MapToDto(Reservation reservation)
    {
        return new ReservationDto
        {
            Id = reservation.Id,
            CustomerName = reservation.CustomerName,
            ReservationDate = reservation.ReservationDate,
            ReservationTime = reservation.ReservationTime,
            TableNumber = reservation.TableNumber,
            NumberOfPeople = reservation.NumberOfPeople,
            CreatedAt = reservation.CreatedAt
        };
    }
}
