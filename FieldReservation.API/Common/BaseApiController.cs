using Microsoft.AspNetCore.Mvc;
using FieldReservation.Application.Common.Results;

namespace FieldReservation.API.Common
{
    [ApiController]
    public abstract class BaseApiController : ControllerBase
    {
        protected IActionResult MapErrors(IReadOnlyList<Error> errors)
        {
            if (errors.Count == 0)
                return StatusCode(StatusCodes.Status500InternalServerError);

            var firstError = errors[0];

            var statusCode = firstError.Type switch
            {
                ErrorType.NotFound => StatusCodes.Status404NotFound,
                ErrorType.Validation => StatusCodes.Status400BadRequest,
                ErrorType.Unauthorized => StatusCodes.Status401Unauthorized,
                ErrorType.Forbidden => StatusCodes.Status403Forbidden,
                ErrorType.InvalidCredentials => StatusCodes.Status401Unauthorized,
                _ => StatusCodes.Status500InternalServerError
            };

            var problem = new ProblemDetails
            {
                Title = GetTitle(firstError.Type),
                Status = statusCode,
                Detail = firstError.Type == ErrorType.Validation
                    ? "One or more validation errors occurred."
                    : firstError.Description
            };

            problem.Extensions["errors"] = errors
                .GroupBy(e => e.Code)
                .ToDictionary(
                    g => g.Key,
                    g => g.Select(e => e.Description).ToArray());

            return StatusCode(statusCode, problem);
        }

        private static string GetTitle(ErrorType type) => type switch
        {
            ErrorType.Validation => "Validation Failed",
            ErrorType.NotFound => "Resource Not Found",
            ErrorType.Unauthorized => "Unauthorized",
            ErrorType.Forbidden => "Forbidden",
            ErrorType.InvalidCredentials => "Invalid Credentials",
            _ => "Server Error"
        };
    }
}