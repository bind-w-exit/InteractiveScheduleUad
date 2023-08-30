using FluentResults;
using InteractiveScheduleUad.Api.Errors;
using Microsoft.AspNetCore.Mvc;

namespace InteractiveScheduleUad.Api.Extensions;

public static class ErrorToObjectResultExtensions
{
    public static ObjectResult ToObjectResult(this IError error)
    {
        return error switch
        {
            NotFoundError notFoundError => new NotFoundObjectResult(notFoundError.Message),
            ValidationError validationError => new BadRequestObjectResult(validationError.ToValidationProblemDetails()),
            _ => new BadRequestObjectResult(error.Message),
        };
    }

    private static ValidationProblemDetails ToValidationProblemDetails(this ValidationError validationError)
    {
        var validationProblemDetails = new ValidationProblemDetails
        {
            //Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1",
            Title = "One or more validation errors occurred.",
            Status = StatusCodes.Status400BadRequest,
        };

        foreach (var error in validationError.Reasons)
        {
            string propertyName = (string)error.Metadata["PropertyName"];

            if (!validationProblemDetails.Errors.TryGetValue(propertyName, out string[]? value))
            {
                validationProblemDetails.Errors[propertyName] = new string[] { error.Message };
            }
            else
            {
                validationProblemDetails.Errors[propertyName] = value.Append(error.Message).ToArray();
            }
        }

        return validationProblemDetails;
    }
}
