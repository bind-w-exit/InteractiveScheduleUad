using FluentResults;

namespace InteractiveScheduleUad.Api.Errors;

public class NotFoundError : Error
{
    public NotFoundError(string entityName) : base($"{entityName} not found")
    {
    }
}
