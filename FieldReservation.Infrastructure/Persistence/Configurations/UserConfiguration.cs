using FieldReservation.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FieldReservation.Infrastructure.Persistence.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<ApplicationUser>
{
    public void Configure(EntityTypeBuilder<ApplicationUser> builder)
    {
        builder.Property(u => u.FullName).HasMaxLength(50).IsRequired();
        builder.Property(u => u.CreatedAt).IsRequired();

        builder.HasMany(u => u.RefreshTokens)
            .WithOne(rf => rf.User)
            .HasForeignKey(rf => rf.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}