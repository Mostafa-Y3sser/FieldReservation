
namespace FieldReservation.Application.Common.Results
{
    public class Result
    {
        private readonly List<Error> _errors = [];

        public bool IsSucceeded { get; }
        public bool IsFailed => !IsSucceeded;
        public IReadOnlyList<Error> Errors => _errors;

        protected Result()
        {
            IsSucceeded = true;
        }
        protected Result(Error error)
        {
            _errors.Add(error);
        }
        protected Result(List<Error> errors)
        {
            _errors.AddRange(errors);
        }

        // Factory Methods
        public static Result Ok() => new();
        public static Result Fail(Error error) => new(error);
        public static Result Fail(List<Error> errors) => new(errors);

        // Implicit Conversions
        public static implicit operator Result(Error error) => Fail(error);
        public static implicit operator Result(List<Error> errors) => Fail(errors);
    }
}