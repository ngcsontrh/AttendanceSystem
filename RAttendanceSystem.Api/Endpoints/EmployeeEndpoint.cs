using RAttendanceSystem.Application.UseCases.EmployeeUC.Commands;

namespace RAttendanceSystem.Api.Endpoints
{
    public static class EmployeeEndpoint
    {
        public static void MapEmployeeEndpoints(this IEndpointRouteBuilder routes)
        {
            var group = routes.MapGroup("/api/employees").WithTags("Employee");

            group.MapPost("", async (CreateEmployeeCommandHandler handler, CreateEmployeeCommand command) =>
            {
                var id = await handler.HandleAsync(command);
                return Results.Created($"/api/employees/{id}", id);
            })
            .WithName("CreateEmployee")
            .WithSummary("Create a new employee")
            .WithDescription("This endpoint creates a new employee in the system and returns the new employee's ID.")
            .Accepts<CreateEmployeeCommand>("application/json")
            .Produces<Guid>(StatusCodes.Status201Created)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status500InternalServerError);
        }
    }
}
