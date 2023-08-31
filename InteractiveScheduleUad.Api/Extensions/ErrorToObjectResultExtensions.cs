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
            ValidationError validationError => new BadRequestObjectResult(validationError.ToValidationProblemDetails()),
            NotFoundError notFoundError => new NotFoundObjectResult(notFoundError.Message),
            EntityAlreadyExistsError entityAlreadyExistsError => new ConflictObjectResult(entityAlreadyExistsError.ToProblemDetails()),
            UnauthorizedError unauthorizedError => new UnauthorizedObjectResult(unauthorizedError.ToProblemDetails()),
            ForbiddenError forbiddenError => new ObjectResult(forbiddenError.ToProblemDetails()) { StatusCode = StatusCodes.Status403Forbidden },
            _ => new BadRequestObjectResult(error.Message),
        };
    }

    private static ValidationProblemDetails ToValidationProblemDetails(this ValidationError validationError)
    {
        var validationProblemDetails = new ValidationProblemDetails()
        {
            //Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1",
            Title = "One or more validation errors occurred.",
            Status = StatusCodes.Status400BadRequest
        };

        foreach (var keyValuePair in validationError.Metadata)
        {
            validationProblemDetails.Errors.Add(keyValuePair.Key, ((List<string>)keyValuePair.Value).ToArray());
        }

        return validationProblemDetails;
    }

    private static ProblemDetails ToProblemDetails(this EntityAlreadyExistsError entityAlreadyExistsError) 
    {
        return new ProblemDetails()
        {
            Title = "Entity Already Exists",
            Detail = entityAlreadyExistsError.Message,
            Status = StatusCodes.Status409Conflict
        };
    }

    private static ProblemDetails ToProblemDetails(this UnauthorizedError unauthorizedError)
    {
        return new ProblemDetails()
        {
            Title = "Invalid Credentials",
            Detail = unauthorizedError.Message,
            Status = StatusCodes.Status401Unauthorized
        };
    }

    private static ProblemDetails ToProblemDetails(this ForbiddenError forbiddenError) 
    {
        return new ProblemDetails()
        {
            Title = "Forbidden",
            Detail = forbiddenError.Message,
            Status = StatusCodes.Status403Forbidden
        };
    }
}