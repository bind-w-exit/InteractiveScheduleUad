using Microsoft.AspNetCore.Mvc;

namespace InteractiveScheduleUad.Api.Controllers.Contracts;

public interface IReactAdminCompatible<T>
{
    public Task<ActionResult<IEnumerable<T>>> GetList(
               [FromQuery] string range = $"[0, 999999]",
               [FromQuery] string sort = "[\"Id\", \"ASC\"]",
               [FromQuery] string filter = "{}");

    //public Task<ActionResult<T>> Get(int id);
}