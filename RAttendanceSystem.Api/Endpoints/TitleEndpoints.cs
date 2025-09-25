using RAttendanceSystem.Application.UseCases.TitleUC.Commands;
using RAttendanceSystem.Application.UseCases.TitleUC.Queries;

namespace RAttendanceSystem.Api.Endpoints
{
    public static class TitleEndpoints
    {
        public static void MapTitleEndpoints(this IEndpointRouteBuilder routes)
        {
            var group = routes.MapGroup("/api/title").WithTags("Title");

            group.MapPost("/", async (
                [FromBody] CreateTitleCommand command,
                [FromServices] CreateTitleCommandHandler handler) =>
            {
                var result = await handler.HandleAsync(command);
                return Results.Created($"/api/title/{result.Id}", result);
            })
            .WithName("CreateTitle")
            .WithSummary("Create a new title")
            .WithDescription("This endpoint creates a new title and returns its ID.")
            .Produces<CreateTitleResponse>(StatusCodes.Status201Created)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status500InternalServerError)
            .RequireAuthorization();

            group.MapPut("/{id}", async (
                [FromRoute] Guid id,
                [FromBody] UpdateTitleCommand request,
                [FromServices] UpdateTitleCommandHandler handler) =>
            {
                var command = request with { Id = id };
                await handler.HandleAsync(command);
                return Results.NoContent();
            })
            .WithName("UpdateTitle")
            .WithSummary("Update a title")
            .WithDescription("This endpoint updates an existing title.")
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status500InternalServerError)
            .RequireAuthorization();

            group.MapGet("/", async (
                [FromServices] GetListTitleQueryHandler handler) =>
            {
                var result = await handler.Handle();
                return Results.Ok(result);
            })
            .WithName("GetTitles")
            .WithSummary("Get a list of titles")
            .Produces<IReadOnlyList<GetListTitleResponseData>>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status500InternalServerError)
            .RequireAuthorization();

            group.MapDelete("/{id}", async (
                [FromRoute] Guid id,
                [FromServices] DeleteTitleCommandHandler handler) =>
            {
                await handler.HandleAsync(new DeleteTitleCommand(id));
                return Results.NoContent();
            })
            .WithName("DeleteTitle")
            .WithSummary("Delete a title")
            .WithDescription("This endpoint deletes a title by ID.")
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status500InternalServerError)
            .RequireAuthorization();
        }
    }
}