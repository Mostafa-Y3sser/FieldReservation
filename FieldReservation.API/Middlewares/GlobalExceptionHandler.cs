using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Diagnostics;
using FieldReservation.Domain.Exceptions;

namespace FieldReservation.API.Middlewares
{
    public class GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
        : IExceptionHandler
    {
        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception,
            CancellationToken cancellationToken)
        {
            logger.LogError(exception, "An unexpected error occurred.");

            if (exception is EmailSendingException emailEx)
            {
                var emailProblem = new ProblemDetails
                {
                    Title = "Email Sending Failed",
                    Status = StatusCodes.Status500InternalServerError,
                    Detail = emailEx.Message
                };

                httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
                await httpContext.Response.WriteAsJsonAsync(emailProblem, cancellationToken);
                return true;
            }

            if (exception is InvalidOperationException operationEx)
            {
                var resultProblem = new ProblemDetails
                {
                    Title = "Invalid Result Operation",
                    Status = StatusCodes.Status500InternalServerError,
                    Detail = operationEx.Message
                };

                httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
                await httpContext.Response.WriteAsJsonAsync(resultProblem, cancellationToken);
                return true;
            }

            var problem = new ProblemDetails
            {
                Title = "Server Error",
                Status = StatusCodes.Status500InternalServerError,
                Detail = "An unexpected error occurred. Please try again later."
            };

            httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
            await httpContext.Response.WriteAsJsonAsync(problem, cancellationToken);

            return true;
        }
    }
}