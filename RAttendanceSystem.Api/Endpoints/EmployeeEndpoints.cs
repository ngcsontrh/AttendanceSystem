using RAttendanceSystem.Application.UseCases.EmployeeUC.Commands;
using RAttendanceSystem.Application.UseCases.EmployeeUC.Queries;

namespace RAttendanceSystem.Api.Endpoints
{
    public static class EmployeeEndpoints
    {
        public static void MapEmployeeEndpoints(this IEndpointRouteBuilder routes)
        {
            var group = routes.MapGroup("/api/employee").WithTags("Employee");

            group.MapPost("/", async (
                [FromBody] CreateEmployeeCommand command,
                [FromServices] CreateEmployeeCommandHandler handler) =>
            {
                var result = await handler.HandleAsync(command);
                return Results.Created($"/api/employee/{result.Id}", result);
            })
            .WithName("CreateEmployee")
            .WithSummary("Create a new employee")
            .WithDescription("This endpoint creates a new employee and returns its ID.")
            .Produces<CreateEmployeeResponse>(StatusCodes.Status201Created)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status500InternalServerError)
            .RequireAuthorization();

            group.MapPut("/{id}", async (
                [FromRoute] Guid id,
                [FromBody] UpdateEmployeeCommand request,
                [FromServices] UpdateEmployeeCommandHandler handler) =>
            {
                var command = request with { Id = id };
                await handler.HandleAsync(command);
                return Results.NoContent();
            })
            .WithName("UpdateEmployee")
            .WithSummary("Update an employee")
            .WithDescription("This endpoint updates an existing employee.")
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status500InternalServerError)
            .RequireAuthorization();

            group.MapGet("/", async (
                [AsParameters] GetPageEmployeeQuery query,
                [FromServices] GetPageEmployeeQueryHandler handler) =>
            {
                var result = await handler.HandleAsync(query);
                return Results.Ok(result);
            })
            .WithName("GetEmployees")
            .WithSummary("Get a paginated list of employees")
            .Produces<GetPageEmployeeResponse>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status500InternalServerError)
            .RequireAuthorization();

            group.MapGet("/basic-info", async (
                [FromServices] GetListEmployeeBasicInfoQueryHandler handler) =>
            {
                var result = await handler.HandleAsync(new GetListEmployeeBasicInfoQuery());
                return Results.Ok(result);
            })
            .WithName("GetEmployeeBasicInfo")
            .WithSummary("Get basic info of employees")
            .Produces<List<GetListEmployeeBasicInfoResponseData>>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status500InternalServerError)
            .RequireAuthorization();

            group.MapGet("/{id}", async (
                [FromRoute] Guid id,
                [FromServices] GetEmployeeDetailQueryHandler handler) =>
            {
                var result = await handler.HandleAsync(new GetEmployeeDetailQuery(id));
                return Results.Ok(result);
            })
            .WithName("GetEmployeeDetail")
            .WithSummary("Get employee detail by ID")
            .Produces<GetEmployeeDetailResponse>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status500InternalServerError)
            .RequireAuthorization();
        }
    }
}
