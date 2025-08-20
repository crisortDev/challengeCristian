using System.Net;
using System.Text.Json;

namespace Challenge.Api.Middlewares;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    public ExceptionHandlingMiddleware(RequestDelegate next) => _next = next;

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (KeyNotFoundException ex)
        {
            await WriteProblem(context, (int)HttpStatusCode.NotFound, "Not Found", ex.Message);
        }
        catch (ArgumentException ex)
        {
            await WriteProblem(context, (int)HttpStatusCode.BadRequest, "Bad Request", ex.Message);
        }
        catch (Exception ex)
        {
            await WriteProblem(context, (int)HttpStatusCode.InternalServerError, "Server Error", ex.Message);
        }
    }

    private static async Task WriteProblem(HttpContext ctx, int status, string title, string detail)
    {
        ctx.Response.StatusCode = status;
        ctx.Response.ContentType = "application/problem+json";
        var payload = new
        {
            type = $"about:blank",
            title,
            status,
            detail,
            instance = ctx.Request.Path.Value
        };
        await ctx.Response.WriteAsync(JsonSerializer.Serialize(payload));
    }
}
