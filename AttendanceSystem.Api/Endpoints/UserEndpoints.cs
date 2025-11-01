using AttendanceSystem.Api.Commons;
using AttendanceSystem.Application.Commons;
using AttendanceSystem.Application.Commons.Errors;
using AttendanceSystem.Application.Commons.Models;
using AttendanceSystem.Application.Features.User.Command;
using AttendanceSystem.Application.Features.User.Queries;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace AttendanceSystem.Api.Endpoints;

public class UserEndpoints : IEndpointRegister
{
    public void MapRoutes(IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/users")
            .WithTags("Users");

        group.MapPost("/register", async (
            [FromBody] CreateUserCommand request,
            [FromServices] CreateUserCommandHandler handler
            ) =>
        {
            var result = await handler.ExecuteAsync(request);
            if (result.IsSuccess)
            {
                return Results.Created($"/api/users/{result.Value}", new { userId = result.Value });
            }
            return Results.InternalServerError(new ErrorData
            {
                Message = result.Errors.First().Message
            });
        }).RequireAuthorization(AppConstraint.AdminPolicy);

        group.MapPost("/login", async (
            [FromBody] LoginCommand request,
            [FromServices] LoginCommandHandler handler
            ) =>
        {
            var result = await handler.ExecuteAsync(request);
            if (result.IsSuccess)
            {
                return Results.Ok(result.Value);
            }
            if (result.HasError<BusinessError>())
            {
                return Results.BadRequest(new ErrorData
                {
                    Message = result.Errors.First().Message
                });
            }
            return Results.InternalServerError(new ErrorData
            {
                Message = result.Errors.First().Message
            });
        });

        group.MapPost("/{userId}/assign-role", async (
            [FromRoute] Guid userId,
            [FromBody] AssignRoleCommand request,
            [FromServices] AssignRoleCommandHandler handler
            ) =>
        {
            var command = request with { UserId = userId };
            var result = await handler.ExecuteAsync(command);
            if (result.IsSuccess)
            {
                return Results.NoContent();
            }
            return Results.InternalServerError(new ErrorData
            {
                Message = result.Errors.First().Message
            });
        })
            .RequireAuthorization(AppConstraint.AdminPolicy);

        group.MapGet("/me", async (
            HttpContext httpContext,
            [FromServices] GetCurrentUserQueryHandler handler
            ) =>
        {
            var userId = httpContext.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null || !Guid.TryParse(userId, out var guid))
            {
                return Results.Unauthorized();
            }
            var result = await handler.ExecuteAsync(new GetCurrentUserQuery(guid));
            if (result.IsSuccess)
            {
                return Results.Ok(result.Value);
            }
            if (result.HasError<NotFoundError>())
            {
                return Results.NotFound(new ErrorData
                {
                    Message = result.Errors.First().Message
                });
            }
            return Results.InternalServerError(new ErrorData
            {
                Message = result.Errors.First().Message
            });
        }).RequireAuthorization(AppConstraint.StaffPolicy);
    }
}
