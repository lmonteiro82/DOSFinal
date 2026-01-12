using DOSFinal.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DOSFinal.Infrastructure.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<Reservation> Reservations { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Reservation>(entity =>
        {
            entity.HasKey(e => e.Id);
            
            entity.Property(e => e.Id)
                .UseIdentityColumn();

            entity.Property(e => e.CustomerName)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(e => e.ReservationDate)
                .IsRequired();

            entity.Property(e => e.ReservationTime)
                .IsRequired();

            entity.Property(e => e.TableNumber)
                .IsRequired();

            entity.Property(e => e.NumberOfPeople)
                .IsRequired();

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("GETDATE()");
        });
    }
}
