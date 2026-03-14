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
        }
    }
}