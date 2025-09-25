using RAttendanceSystem.Application;
using RAttendanceSystem.Application.UseCases.SystemNotificationUC.Queries;
using System.Security.Claims;

namespace RAttendanceSystem.Api.Endpoints
{
    public static class SystemNotificationEndpoints
    {
        public static void MapSystemNotificationEndpoints(this IEndpointRouteBuilder routes)
        {
            var group = routes.MapGroup("/api/system-notification").WithTags("SystemNotification");

            group.MapGet("/", async (
                [AsParameters] GetPageSystemNotificationQuery model,
                [FromServices] GetPageSystemNotificationQueryHandler handler,
                HttpContext httpContext) =>
            {
                var receiverId = httpContext.User.FindFirstValue(AppConstraint.EmployeeIdClaim);
                if (receiverId == null)
                {
                    return Results.Unauthorized();
                }
                var query = model with { ReceiverId = Guid.Parse(receiverId) };
                var result = await handler.HandleAsync(query);
                return Results.Ok(result);
            })
            .WithName("GetSystemNotifications")
            .WithSummary("Get a paginated list of system notifications")
            .WithDescription("This endpoint returns a paginated list of system notifications")
            .Produces<GetPageSystemNotificationResponse>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status500InternalServerError)
            .RequireAuthorization();
        }
    }
}