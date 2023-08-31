using FluentResults;

namespace InteractiveScheduleUad.Api.Errors;

public class UnauthorizedError : Error
{
    public UnauthorizedError(string message) : base(message)
    {
    }
}