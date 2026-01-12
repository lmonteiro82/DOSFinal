using DOSFinal.Domain.Entities;
using DOSFinal.Domain.Interfaces;
using DOSFinal.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace DOSFinal.Infrastructure.Repositories;

public class ReservationRepository : IReservationRepository
{
    private readonly ApplicationDbContext _context;

    public ReservationRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Reservation?> GetByIdAsync(int id)
    {
        return await _context.Reservations.FindAsync(id);
    }

    public async Task<IEnumerable<Reservation>> GetAllAsync()
    {
        return await _context.Reservations
            .OrderBy(r => r.ReservationDate)
            .ThenBy(r => r.ReservationTime)
            .ToListAsync();
    }

    public async Task<IEnumerable<Reservation>> GetByDateAsync(DateOnly date)
    {
        return await _context.Reservations
            .Where(r => r.ReservationDate == date)
            .OrderBy(r => r.ReservationTime)
            .ToListAsync();
    }

    public async Task<Reservation> CreateAsync(Reservation reservation)
    {
        _context.Reservations.Add(reservation);
        await _context.SaveChangesAsync();
        return reservation;
    }

    public async Task<Reservation?> UpdateAsync(Reservation reservation)
    {
        var existing = await _context.Reservations.FindAsync(reservation.Id);
        if (existing == null)
            return null;

        _context.Entry(existing).CurrentValues.SetValues(reservation);
        await _context.SaveChangesAsync();
        return existing;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var reservation = await _context.Reservations.FindAsync(id);
        if (reservation == null)
            return false;

        _context.Reservations.Remove(reservation);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> HasConflictAsync(DateOnly date, TimeOnly time, int tableNumber, int? excludeReservationId = null)
    {
        var query = _context.Reservations
            .Where(r => r.ReservationDate == date &&
                       r.ReservationTime == time &&
                       r.TableNumber == tableNumber);

        if (excludeReservationId.HasValue)
        {
            query = query.Where(r => r.Id != excludeReservationId.Value);
        }

        return await query.AnyAsync();
    }
}
