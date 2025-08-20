using Challenge.Core.Abstraction.Services;
using Challenge.Core.DTOs.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Api.Endpoints;

public static class TasksEndpoints
{
    public static void MapTasksEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/tasks").WithTags("Tasks");

        group.MapPost("/", async (
            [FromBody] CreateTaskDto dto,
            [FromServices] ITaskService service,
            CancellationToken ct) =>
        {
            var task = await service.CreateAsync(dto, ct);
            return Results.Created($"/tasks/{task.Id}", task);
        });

        group.MapGet("/{id:guid}", async (
            Guid id,
            [FromServices] ITaskService service,
            CancellationToken ct) =>
        {
            var task = await service.GetAsync(id, ct);
            return task is null ? Results.NotFound() : Results.Ok(task);
        });

        group.MapGet("/", async (
            Guid? assigneeId,
            [FromServices] ITaskService service,
            CancellationToken ct) =>
        {
            var tasks = await service.ListAsync(assigneeId, ct);
            return Results.Ok(tasks);
        });

        group.MapPut("/{id:guid}", async (
            Guid id,
            [FromBody] UpdateTaskDto dto,
            [FromServices] ITaskService service,
            CancellationToken ct) =>
        {
            await service.UpdateAsync(id, dto, ct);
            return Results.NoContent();
        });

        group.MapPatch("/{id:guid}/status", async (
            Guid id,
            [FromBody] ChangeTaskStatusDto dto,
            [FromServices] ITaskService service,
            CancellationToken ct) =>
        {
            await service.ChangeStatusAsync(id, dto, ct);
            return Results.NoContent();
        });

        group.MapPatch("/{id:guid}/reassign", async (
            Guid id,
            [FromBody] ReassignTaskDto dto,
            [FromServices] ITaskService service,
            CancellationToken ct) =>
        {
            await service.ReassignAsync(id, dto, ct);
            return Results.NoContent();
        });

        group.MapDelete("/{id:guid}", async (
            Guid id,
            [FromServices] ITaskService service,
            CancellationToken ct) =>
        {
            await service.DeleteAsync(id, ct);
            return Results.NoContent();
        });
    }
}
