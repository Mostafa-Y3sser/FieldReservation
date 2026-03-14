using Mapster;
using FieldReservation.Domain.Entities;

namespace FieldReservation.Application.Reservations.Queries.GetReservation
{
    public class ReservationMappingConfig : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Reservation, ReservationResponse>();
        }
    }
}