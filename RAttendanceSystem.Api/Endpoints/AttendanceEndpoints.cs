using RAttendanceSystem.Application.UseCases.AttendanceUC.Commands;
using RAttendanceSystem.Application.UseCases.AttendanceUC.Queries;
using System.Security.Claims;
using RAttendanceSystem.Application;

namespace RAttendanceSystem.Api.Endpoints
{
    public static class AttendanceEndpoints
    {
        public static void MapAttendanceEndpoints(this IEndpointRouteBuilder routes)
        {
            var group = routes.MapGroup("/api/attendance").WithTags("Attendance");

            group.MapPost("/check-in", async (
                [FromBody] CheckInCommand request,
                [FromServices] CheckInCommandHandler handler,
                HttpContext httpContext) =>
            {
                var employeeId = httpContext.User.FindFirstValue(AppConstraint.EmployeeIdClaim);
                if (string.IsNullOrEmpty(employeeId))
                {
                    return Results.Unauthorized();
                }
                var command = request with { EmployeeId = Guid.Parse(employeeId) };
                var result = await handler.HandleAsync(command);
                return Results.Ok(result);
            })
            .WithName("CheckIn")
            .WithSummary("Check in for attendance")
            .WithDescription("This endpoint allows employees to check in for attendance. Requires valid WiFi network.")
            .Produces<AttendanceResponse>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status500InternalServerError)
            .RequireAuthorization();

            group.MapPost("/check-out", async (
                [FromBody] CheckOutCommand request,
                [FromServices] CheckOutCommandHandler handler,
                HttpContext httpContext) =>
            {
                var employeeId = httpContext.User.FindFirstValue(AppConstraint.EmployeeIdClaim);
                if (string.IsNullOrEmpty(employeeId))
                {
                    return Results.Unauthorized();
                }
                var command = request with { EmployeeId = Guid.Parse(employeeId) };
                var result = await handler.HandleAsync(command);
                return Results.Ok(result);
            })
            .WithName("CheckOut")
            .WithSummary("Check out for attendance")
            .WithDescription("This endpoint allows employees to check out for attendance. Requires valid WiFi network.")
            .Produces<AttendanceResponse>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status500InternalServerError)
            .RequireAuthorization();

            group.MapGet("/history", async (
                [AsParameters] GetAttendanceHistoryQuery query,
                [FromServices] GetAttendanceHistoryQueryHandler handler,
                HttpContext httpContext) =>
            {
                var isManager = httpContext.User.IsInRole(AppConstraint.ManagerRoleName);
                if (!isManager && !query.EmployeeId.HasValue)
                {
                    return Results.BadRequest("EmployeeId is required for non-manager users.");
                }
                var result = await handler.HandleAsync(query);
                return Results.Ok(result);
            })
            .WithName("GetAttendanceHistory")
            .WithSummary("Get attendance history")
            .WithDescription("This endpoint returns attendance history with various filters including date, month, status, and location. No pagination - returns all matching records.")
            .Produces<GetAttendanceHistoryResponse>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status500InternalServerError)
            .RequireAuthorization();
        }       
    }
}