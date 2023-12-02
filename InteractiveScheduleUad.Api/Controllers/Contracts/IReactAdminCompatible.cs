using Microsoft.AspNetCore.Mvc;

namespace InteractiveScheduleUad.Api.Controllers.Contracts;

/// <summary>
/// Defines methods that are required for a controller to be compatible with react-admin's
/// "Simple REST" data provider:
/// https://github.com/marmelab/react-admin/tree/master/packages/ra-data-simple-rest
/// </summary>
public interface IReactAdminCompatible<T>
{
    public Task<ActionResult<IEnumerable<T>>> GetList(
               [FromQuery] string range = $"[0, 999999]",
               [FromQuery] string sort = "[\"Id\", \"ASC\"]",
               [FromQuery] string filter = "{}");
}