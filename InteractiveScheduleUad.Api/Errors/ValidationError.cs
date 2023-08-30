using FluentResults;

namespace InteractiveScheduleUad.Api.Errors;

public class ValidationError : Error
{
    public ValidationError(string message = "One or more validation errors occurred") : base(message)
    {
    }
}