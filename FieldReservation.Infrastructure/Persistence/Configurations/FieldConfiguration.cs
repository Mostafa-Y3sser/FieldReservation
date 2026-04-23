using Microsoft.EntityFrameworkCore;
using FieldReservation.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FieldReservation.Infrastructure.Persistence.Configurations
{
    internal class FieldConfiguration : IEntityTypeConfiguration<Field>
    {
        public void Configure(EntityTypeBuilder<Field> builder)
        {
            builder.HasKey(x => x.Id);
            builder.HasIndex(x => x.Name);
        }
    }
}
