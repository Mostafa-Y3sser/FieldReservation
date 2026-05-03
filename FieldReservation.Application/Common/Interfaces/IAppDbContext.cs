using Microsoft.EntityFrameworkCore;
using FieldReservation.Domain.Entities;

namespace FieldReservation.Application.Common.Interfaces
{
    public interface IAppDbContext
    {
        DbSet<Field> Fields { get; }
        DbSet<Reservation> Reservations { get; }
        DbSet<RefreshToken> RefreshTokens { get; }
        DbSet<ApplicationUser> Users { get; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}