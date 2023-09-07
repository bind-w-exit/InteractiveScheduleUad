using FluentResults;

namespace InteractiveScheduleUad.Api.Errors;

public class ForbiddenError : Error
{
    public ForbiddenError(string message) : base(message)
    {
    }
}