using FluentResults;

namespace InteractiveScheduleUad.Api.Errors;

public class EntityAlreadyExistsError : Error
{
    public EntityAlreadyExistsError(string entityName) : base($"{entityName} already exists")
    {
    }
}