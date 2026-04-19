using MediatR;
using Microsoft.AspNetCore.Mvc;
using FieldReservation.API.Common;
using FieldReservation.Application.Auth.Commands.Login;
using Microsoft.AspNetCore.Authorization;
using FieldReservation.Application.Auth.Commands.Register;
using FieldReservation.Application.Auth.Commands.EmailVerification;
using FieldReservation.Application.Auth.Commands.ForgotPassword;
using FieldReservation.Application.Auth.Commands.GoogleSignIn;
using FieldReservation.Application.Auth.Commands.GoogleSignUp;
using FieldReservation.Application.Auth.Commands.RefreshToken;
using FieldReservation.Application.Auth.Commands.ResetPassword;
using FieldReservation.Application.Auth.Commands.RevokeRefreshToken;
using FieldReservation.Application.Auth.Commands.SendEmailVerificationToken;
using Microsoft.AspNetCore.RateLimiting;
using FieldReservation.Application.Auth.Dtos;

namespace FieldReservation.API.Controllers;

[Route("api/[controller]")]
[EnableRateLimiting("auth")]
public class AuthController(ISender sender) : BaseApiController
{
    [AllowAnonymous]
    [HttpPost("register")]
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Register(
        [FromBody] RegisterCommand command,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(command, cancellationToken);
        return HandleResult<AuthResponseDto>(result);
    }

    [AllowAnonymous]
    [HttpPost("login")]
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Login(
        [FromBody] LoginCommand command,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(command, cancellationToken);
        return HandleResult<AuthResponseDto>(result);
    }

    [AllowAnonymous]
    [HttpPost("send-email-verification-token")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> SendEmailVerificationToken(
        [FromBody] SendEmailVerificationTokenCommand command,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(command, cancellationToken);
        return HandleResult(result);
    }

    [AllowAnonymous]
    [HttpPost("email-verification")]
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> EmailVerification(
        [FromBody] EmailVerificationCommand command,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(command, cancellationToken);
        return HandleResult<AuthResponseDto>(result);
    }

    [AllowAnonymous]
    [HttpPost("forgot-password")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> ForgotPassword(
        [FromBody] ForgotPasswordCommand command,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(command, cancellationToken);
        return HandleResult(result);
    }

    [AllowAnonymous]
    [HttpPost("reset-password")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> ResetPassword(
        [FromBody] ResetPasswordCommand command,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(command, cancellationToken);
        return HandleResult(result);
    }

    [AllowAnonymous]
    [HttpPost("google-signin")]
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GoogleSignIn(
        [FromBody] GoogleSignInCommand command,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(command, cancellationToken);
        return HandleResult<AuthResponseDto>(result);
    }

    [AllowAnonymous]
    [HttpPost("google-signup")]
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GoogleSignUp(
        [FromBody] GoogleSignUpCommand command,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(command, cancellationToken);
        return HandleResult<AuthResponseDto>(result);
    }

    [AllowAnonymous]
    [HttpPost("refresh-token")]
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> RefreshToken(
        [FromBody] RefreshTokenCommand command,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(command, cancellationToken);
        return HandleResult<AuthResponseDto>(result);
    }

    [Authorize]
    [HttpPost("revoke-refresh-token")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> RevokeRefreshToken(
        [FromBody] RevokeRefreshTokenCommand command,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(command, cancellationToken);
        return HandleResult(result);
    }
}