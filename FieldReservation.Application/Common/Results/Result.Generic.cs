using FieldReservation.Domain.Exceptions;

namespace FieldReservation.Application.Common.Results
{
    public class Result<TValue> : Result
    {
        private readonly TValue _value;

        public TValue Value => IsSucceeded
            ? _value
            : throw new InvalidOperationException("Operation Is Not Succeeded");

        private Result(TValue value) : base()
        {
            _value = value;
        }
        private Result(Error error) : base(error)
        {
            _value = default!;
        }
        private Result(List<Error> errors) : base(errors)
        {
            _value = default!;
        }

        // Factory Methods
        public static Result<TValue> Ok(TValue value) => new(value);
        public new static Result<TValue> Fail(Error error) => new(error);
        public new static Result<TValue> Fail(List<Error> errors) => new(errors);

        // Implicit Conversions
        public static implicit operator Result<TValue>(TValue value) => Ok(value);
        public static implicit operator Result<TValue>(Error error) => Fail(error);
        public static implicit operator Result<TValue>(List<Error> errors) => Fail(errors);
    }
}