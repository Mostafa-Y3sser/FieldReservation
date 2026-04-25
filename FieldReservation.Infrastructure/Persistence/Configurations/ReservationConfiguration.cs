using Microsoft.EntityFrameworkCore;
using FieldReservation.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FieldReservation.Infrastructure.Persistence.Configurations
{
    public class ReservationConfiguration : IEntityTypeConfiguration<Reservation>
    {
        public void Configure(EntityTypeBuilder<Reservation> builder)
        {
            builder.HasKey(r => r.Id);

            builder.Property(r => r.FieldId)
                .IsRequired();

            builder.Property(r => r.UserId)
                .IsRequired();

            builder.Property(r => r.StartTime)
                .IsRequired();

            builder.Property(r => r.EndTime)
                .IsRequired();

            builder.Property(r => r.StripeSessionId)
                .HasMaxLength(255)
                .IsRequired(false);

            builder.Property(r => r.StripePaymentIntentId)
                .HasMaxLength(255)
                .IsRequired(false);

            // Index for fast webhook lookup by Stripe session ID
            builder.HasIndex(r => r.StripeSessionId);

            builder.Property(r => r.RowVersion)
                .IsRowVersion();

            builder.HasOne(r => r.Field)
                .WithMany(f => f.Reservations)
                .HasForeignKey(r => r.FieldId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(r => r.User)
                .WithMany(u => u.Reservations)
                .HasForeignKey(r => r.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}