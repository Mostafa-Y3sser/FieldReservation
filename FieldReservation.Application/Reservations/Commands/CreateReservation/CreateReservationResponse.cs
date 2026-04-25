namespace FieldReservation.Application.Reservations.Commands.CreateReservation
{
    /// <summary>Response returned when a reservation is created successfully.</summary>
    /// <param name="ReservationId">The newly created reservation ID.</param>
    /// <param name="CheckoutUrl">The Stripe hosted checkout URL the client must redirect the user to.</param>
    public record CreateReservationResponse(Guid ReservationId, string CheckoutUrl);
}
