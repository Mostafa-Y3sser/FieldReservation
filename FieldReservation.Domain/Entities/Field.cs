
namespace FieldReservation.Domain.Entities;

public class Field : BaseEntity
{
    public string Name { get; private set; } = string.Empty;
    public decimal HourlyRate { get; private set; }

    public static Field Create(string name, decimal hourlyRate)
    {
        return new Field
        {
            Name = name,
            HourlyRate = hourlyRate
        };
    }

    // Navigation Properties
    public ICollection<Reservation>? Reservations { get; private set; }
}