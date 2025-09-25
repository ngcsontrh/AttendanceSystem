using RAttendanceSystem.Application.UseCases.DepartmentUC.Commands;
using RAttendanceSystem.Application.UseCases.DepartmentUC.Queries;

namespace RAttendanceSystem.Api.Endpoints
{
    public static class DepartmentEndpoints
    {
        public static void MapDepartmentEndpoints(this IEndpointRouteBuilder routes)
        {
            var group = routes.MapGroup("/api/department").WithTags("Department");

            group.MapPost("/", async (
                [FromBody] CreateDepartmentCommand command,
                [FromServices] CreateDepartmentCommandHandler handler) =>
            {
                var result = await handler.HandleAsync(command);
                return Results.Created($"/api/department/{result.Id}", result);
            })
            .WithName("CreateDepartment")
            .WithSummary("Create a new department")
            .WithDescription("This endpoint creates a new department and returns its ID.")
            .Produces<CreateDepartmentResponse>(StatusCodes.Status201Created)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status500InternalServerError)
            .RequireAuthorization();

            group.MapPut("/{id}", async (
                [FromRoute] Guid id,
                [FromBody] UpdateDepartmentCommand request,
                [FromServices] UpdateDepartmentCommandHandler handler) =>
            {
                var command = request with { Id = id };
                await handler.HandleAsync(command);
                return Results.NoContent();
            })
            .WithName("UpdateDepartment")
            .WithSummary("Update a department")
            .WithDescription("This endpoint updates an existing department.")
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status500InternalServerError)
            .RequireAuthorization();

            group.MapGet("/", async (
                [FromServices] GetListDepartmentQueryHandler handler) =>
            {
                var result = await handler.Handle(new GetListDepartmentQuery());
                return Results.Ok(result);
            })
            .WithName("GetDepartments")
            .WithSummary("Get a list of departments")
            .Produces<IReadOnlyList<GetListDepartmentResponseData>>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status500InternalServerError)
            .RequireAuthorization();

            group.MapDelete("/{id}", async (
                [FromRoute] Guid id,
                [FromServices] DeleteDepartmentCommandHandler handler) =>
            {
                await handler.HandleAsync(new DeleteDepartmentCommand(id));
                return Results.NoContent();
            })
            .WithName("DeleteDepartment")
            .WithSummary("Delete a department")
            .WithDescription("This endpoint deletes a department by ID.")
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status500InternalServerError)
            .RequireAuthorization();
        }
    }
}