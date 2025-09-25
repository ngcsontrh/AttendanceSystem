using RAttendanceSystem.Application.UseCases.AttendanceWiFiUC.Commands;
using RAttendanceSystem.Application.UseCases.AttendanceWiFiUC.Queries;

namespace RAttendanceSystem.Api.Endpoints
{
    public static class AttendanceWiFiEndpoints
    {
        public static void MapAttendanceWiFiEndpoints(this IEndpointRouteBuilder routes)
        {
            var group = routes.MapGroup("/api/attendance-wifi").WithTags("AttendanceWiFi");

            group.MapPost("/", async (
                [FromBody] CreateAttendanceWiFiCommand command,
                [FromServices] CreateAttendanceWiFiCommandHandler handler) =>
            {
                var result = await handler.HandleAsync(command);
                return Results.Created($"/api/attendance-wifi/{result.Id}", result);
            })
            .WithName("CreateAttendanceWiFi")
            .WithSummary("Create a new AttendanceWiFi")
            .WithDescription("This endpoint creates a new AttendanceWiFi and returns its ID.")
            .Produces<CreateAttendanceWiFiResponse>(StatusCodes.Status201Created)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status500InternalServerError)
            .RequireAuthorization();

            group.MapPut("/{id}", async (
                [FromRoute] Guid id,
                [FromBody] UpdateAttendanceWiFiCommand request,
                [FromServices] UpdateAttendanceWiFiCommandHandler handler) =>
            {
                var command = request with { Id = id };
                await handler.HandleAsync(command);
                return Results.NoContent();
            })
            .WithName("UpdateAttendanceWiFi")
            .WithSummary("Update AttendanceWiFi")
            .WithDescription("This endpoint updates an existing AttendanceWiFi.")
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status500InternalServerError)
            .RequireAuthorization();

            group.MapGet("/", async (
                [FromServices] GetListAttendanceWiFiQueryHandler handler) =>
            {
                var result = await handler.Handle(new GetListAttendanceWiFiQuery());
                return Results.Ok(result);
            })
            .WithName("GetAttendanceWiFis")
            .WithSummary("Get a list of AttendanceWiFi records")
            .Produces<IReadOnlyList<GetListAttendanceWiFiResponseData>>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status500InternalServerError)
            .RequireAuthorization();

            group.MapGet("/{id}", async (
                [FromRoute] Guid id,
                [FromServices] GetAttendanceWiFiDetailQueryHandler handler) =>
            {
                var result = await handler.HandleAsync(new GetAttendanceWiFiDetailQuery(id));
                return Results.Ok(result);
            })
            .WithName("GetAttendanceWiFiDetail")
            .WithSummary("Get AttendanceWiFi detail by ID")
            .Produces<GetAttendanceWiFiDetailResponse>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status500InternalServerError)
            .RequireAuthorization();

            group.MapDelete("/{id}", async (
                [FromRoute] Guid id,
                [FromServices] DeleteAttendanceWiFiCommandHandler handler) =>
            {
                await handler.HandleAsync(new DeleteAttendanceWiFiCommand(id));
                return Results.NoContent();
            })
            .WithName("DeleteAttendanceWiFi")
            .WithSummary("Delete AttendanceWiFi")
            .WithDescription("This endpoint deletes an AttendanceWiFi by ID.")
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status500InternalServerError)
            .RequireAuthorization();
        }
    }
}
