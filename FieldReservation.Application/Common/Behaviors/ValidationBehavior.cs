using MediatR;
using FluentValidation;
using FieldReservation.Application.Common.Results;

namespace FieldReservation.Application.Common.Behaviors
{
    public class ValidationBehavior<TRequest, TResponse>(IEnumerable<IValidator<TRequest>> validators)
        : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next,
            CancellationToken cancellationToken)
        {
            if (!validators.Any())
                return await next();

            var context = new ValidationContext<TRequest>(request);

            var validationResults = await Task.WhenAll(
                validators.Select(v => v.ValidateAsync(context, cancellationToken)));

            var failures = validationResults
                .SelectMany(r => r.Errors)
                .Where(f => f is not null)
                .ToList();

            if (failures.Count == 0)
                return await next();

            var errors = failures
                .Select(f => Error.Validation(f.PropertyName, f.ErrorMessage))
                .Distinct()
                .ToList();

            return CreateValidationResult<TResponse>(errors);
        }

        private static TResponse CreateValidationResult<TResult>(List<Error> errors)
        {
            if (typeof(TResult) == typeof(Result))
                return (TResponse)(object)Result.Fail(errors);

            var failMethod = typeof(TResult).GetMethod("Fail", new[] { typeof(List<Error>) });

            if (failMethod is not null)
                return (TResponse)failMethod.Invoke(null, new object[] { errors })!;

            throw new InvalidOperationException(
                $"The return type {typeof(TResult).Name} is not a valid Result type.");
        }
    }
}