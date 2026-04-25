using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Stripe;
using Stripe.Checkout;
using FieldReservation.API.Common;
using FieldReservation.Application.Reservations.Commands.ConfirmPayment;
using FieldReservation.Application.Settings;

namespace FieldReservation.API.Controllers;

[Route("api/[controller]")]
public class PaymentsController(ISender sender, IOptions<StripeSettings> stripeOptions) : BaseApiController
{
    /// <summary>
    /// Stripe webhook endpoint — receives payment lifecycle events from Stripe.
    /// Must be [AllowAnonymous] because Stripe authenticates via the webhook signature header,
    /// not via our JWT. The raw request body is required for signature verification.
    /// </summary>
    [AllowAnonymous]
    [HttpPost("webhook")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Webhook(CancellationToken cancellationToken)
    {
        string payload;
        using (var reader = new StreamReader(Request.Body))
        {
            payload = await reader.ReadToEndAsync(cancellationToken);
        }

        var signatureHeader = Request.Headers["Stripe-Signature"].FirstOrDefault();
        if (string.IsNullOrWhiteSpace(signatureHeader))
            return BadRequest("Missing Stripe-Signature header.");

        Event stripeEvent;
        try
        {
            stripeEvent = EventUtility.ConstructEvent(
                payload,
                signatureHeader,
                stripeOptions.Value.WebhookSecret,
                throwOnApiVersionMismatch: false);
        }
        catch (StripeException ex)
        {
            return BadRequest($"Webhook signature verification failed: {ex.Message}");
        }

        if (stripeEvent.Type == EventTypes.CheckoutSessionCompleted)
        {
            if (stripeEvent.Data.Object is not Session session)
                return BadRequest("Unexpected event data object type.");

            if (string.IsNullOrWhiteSpace(session.PaymentIntentId))
                return BadRequest("Session is missing PaymentIntentId.");

            var result = await sender.Send(
                new ConfirmPaymentCommand(session.Id, session.PaymentIntentId),
                cancellationToken);

            if (result.IsFailed)
                return Ok(new { warning = result.Errors.FirstOrDefault()?.Description });
        }

        return Ok();
    }
}
