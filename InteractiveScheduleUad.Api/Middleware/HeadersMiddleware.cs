using Azure;

namespace InteractiveScheduleUad.Api.Middleware;

public class HeadersMiddleware
{
    private readonly RequestDelegate _next;

    public HeadersMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        // Add your custom headers here
        context.Response.Headers.Add("Access-Control-Expose-Headers", "Content-Range, X-Total-Count");

        // Call the next middleware in the pipeline
        await _next(context);
    }
}