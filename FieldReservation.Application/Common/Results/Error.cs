
namespace FieldReservation.Application.Common.Results
{
    public class Error
    {
        public string Code { get; private set; }
        public string Description { get; private set; }
        public ErrorType Type { get; set; }

        private Error(string code, string description, ErrorType errorType)
        {
            Code = code;
            Description = description;
            Type = errorType;
        }

        public static Error Failure(
            string code = "Error.Failure",
            string description = "General Failure Has Occurred")
            => new(code, description, ErrorType.Failure);

        public static Error NotFound(
            string code = "Error.NotFound",
            string description = "Requested Resource Was Not Found")
            => new(code, description, ErrorType.NotFound);

        public static Error Unauthorized(
            string code = "Error.Unauthorized",
            string description = "Authentication Is Required")
            => new(code, description, ErrorType.Unauthorized);

        public static Error Validation(
            string code = "Error.Validation",
            string description = "Validation Failed")
            => new(code, description, ErrorType.Validation);

        public static Error Forbidden(
            string code = "Error.Forbidden",
            string description = "Access Is Forbidden")
            => new(code, description, ErrorType.Forbidden);

        public static Error InvalidCredentials(
            string code = "Error.InvalidCredentials",
            string description = "Invalid Credentials Provided")
            => new(code, description, ErrorType.InvalidCredentials);
    }
}