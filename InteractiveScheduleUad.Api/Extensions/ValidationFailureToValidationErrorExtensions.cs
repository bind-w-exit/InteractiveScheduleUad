using FluentValidation.Results;
using InteractiveScheduleUad.Api.Errors;

namespace InteractiveScheduleUad.Api.Extensions;

public static class ValidationFailureToValidationErrorExtensions
{
    public static ValidationError ToValidationError(this IEnumerable<ValidationFailure> failures)
    {
        var validationError = new ValidationError();

        foreach (var failure in failures)
        {
            if (!validationError.Metadata.TryGetValue(failure.PropertyName, out object? value))
            {
                value = new List<string>() { failure.ErrorMessage };
                validationError.Metadata.TryAdd(failure.PropertyName, value);
            }
            else if (value is List<string> errorList)
            {
                errorList.Add(failure.ErrorMessage);
            }
        }

        return validationError;
    }
}