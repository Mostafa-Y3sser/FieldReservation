using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using FieldReservation.Application.Common.Results;

namespace FieldReservation.API.Common
{
    [ApiController]
    public abstract class BaseApiController : ControllerBase
    {
        protected IActionResult HandleResult(Result result)
        {
            if (result.IsSucceeded)
                return Ok();
            else
                return HandleErrors(result.Errors);

        }
        protected ActionResult HandleResult<TValue>(Result<TValue> result)
        {
            if (result.IsSucceeded)
                return Ok(result.Value);
            else
                return HandleErrors(result.Errors);
        }
        private ActionResult HandleErrors(IReadOnlyList<Error> errors)
        {


            if (errors.Count == 0)
                return Problem(statusCode: StatusCodes.Status500InternalServerError, title: "An UnExpected Error Occurred");
            else if (errors.All(error => error.Type == ErrorType.Validation))
                return HandleValidatioErrors(errors);
            else
                return HandleSingleError(errors[0]);

        }

        private ActionResult HandleValidatioErrors(IReadOnlyList<Error> errors)
        {
            var modelState = new ModelStateDictionary();
            foreach (var error in errors)
            {
                modelState.AddModelError(error.Code, error.Description);
            }
            return ValidationProblem(modelState);
        }
        private ActionResult HandleSingleError(Error error)
        {
            return Problem
                (
                    title: error.Code,
                    type: error.Type.ToString(),
                    detail: error.Description,
                    instance: Request.Path,
                    statusCode: GetStatusCodeByErrorType(error.Type)
                );
        }


        private int GetStatusCodeByErrorType(ErrorType errorType)
        {
            return errorType switch
            {
                ErrorType.Validation => StatusCodes.Status400BadRequest,
                ErrorType.NotFound => StatusCodes.Status404NotFound,
                ErrorType.Unauthorized => StatusCodes.Status401Unauthorized,
                ErrorType.InvalidCredentials => StatusCodes.Status401Unauthorized,
                ErrorType.Forbidden => StatusCodes.Status403Forbidden,
                ErrorType.Conflict => StatusCodes.Status409Conflict,
                ErrorType.Failure => StatusCodes.Status500InternalServerError,
                _ => StatusCodes.Status500InternalServerError

            };
        }
    }
}