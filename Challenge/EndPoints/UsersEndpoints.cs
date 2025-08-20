using Challenge.Core.Abstraction.Services;
using Challenge.Core.DTOs.Users;

namespace Challenge.Api;

public static class UsersEndpoints
{
    public static IEndpointRouteBuilder MapUsersEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/users").WithTags("Users");

        group.MapGet("/", async (IUserService svc, CancellationToken ct) =>
        {
            var list = await svc.ListAsync(ct);
            return Results.Ok(list);
        });

        group.MapGet("/{id:guid}", async (Guid id, IUserService svc, CancellationToken ct) =>
        {
            var dto = await svc.GetAsync(id, ct);
            return dto is null ? Results.NotFound() : Results.Ok(dto);
        });

        group.MapPost("/", async (CreateUserDto body, IUserService svc, CancellationToken ct) =>
        {
            var dto = await svc.CreateAsync(body, ct);
            return Results.Created($"/api/users/{dto.Id}", dto);
        });

        group.MapPut("/{id:guid}", async (Guid id, UpdateUserDto body, IUserService svc, CancellationToken ct) =>
        {
            await svc.UpdateAsync(id, body, ct);
            return Results.NoContent();
        });

        group.MapDelete("/{id:guid}", async (Guid id, IUserService svc, CancellationToken ct) =>
        {
            await svc.DeleteAsync(id, ct);
            return Results.NoContent();
        });

        return routes;
    }
}
