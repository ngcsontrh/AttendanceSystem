using AttendanceSystem.Api.Commons;
using AttendanceSystem.Application.Commons;
using AttendanceSystem.Application.Commons.Errors;
using AttendanceSystem.Application.Features.LeaveRequest.Commands;
using AttendanceSystem.Application.Features.LeaveRequest.Queries;
using Microsoft.AspNetCore.Mvc;
using AttendanceSystem.Application.Commons.Models;

namespace AttendanceSystem.Api.Endpoints;

public class LeaveRequestEndpoints : IEndpointRegister
{
    public void MapRoutes(IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/leave-requests")
            .WithTags("LeaveRequests")
            .RequireAuthorization();

        group.MapGet("/", async (
            [AsParameters] GetPageLeaveRequestQuery request,
            [FromServices] GetPageLeaveRequestQueryHandler handler) =>
        {
            var result = await handler.ExecuteAsync(request);

            if (result.IsSuccess)
            {
                return Results.Ok(result.Value);
            }

            return Results.InternalServerError(new ErrorData
            {
                Message = result.Errors.First().Message,
            });
        }).RequireAuthorization(AppConstraint.StaffPolicy);

        group.MapGet("/{id}", async (
            [FromRoute] Guid id,
            [FromServices] GetLeaveRequestByIdQueryHandler handler) =>
        {
            var result = await handler.ExecuteAsync(new GetLeaveRequestByIdQuery(id));
            if (result.IsSuccess)
            {
                return Results.Ok(result.Value);
            }
            else if (result.HasError<NotFoundError>())
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

        group.MapPost("/", async (
            [FromBody] CreateLeaveRequestCommand request,
            [FromServices] CreateLeaveRequestCommandHandler handler
            ) =>
        {
            var result = await handler.ExecuteAsync(request);
            if (result.IsSuccess)
            {
                return Results.Created($"/api/leave-requests/{result.Value.Id}", result.Value);
            }
            return Results.InternalServerError(new ErrorData
            {
                Message = result.Errors.First().Message
            });
        }).RequireAuthorization(AppConstraint.StaffPolicy);

        group.MapPost("/approve", async (
            [FromBody] ApproveLeaveRequestCommand request,
            [FromServices] ApproveLeaveRequestCommandHandler handler
            ) =>
        {
            var result = await handler.ExecuteAsync(request);
            if (result.IsSuccess)
            {
                return Results.Ok();
            }
            else if (result.HasError<NotFoundError>())
            {
                return Results.NotFound(new ErrorData
                {
                    Message = result.Errors.First().Message
                });
            }
            else if (result.HasError<BusinessError>())
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
        }).RequireAuthorization(AppConstraint.ManagerPolicy);

        group.MapPost("/reject", async (
            [FromBody] RejectLeaveRequestCommand request,
            [FromServices] RejectLeaveRequestCommandHandler handler
            ) =>
        {
            var result = await handler.ExecuteAsync(request);
            if (result.IsSuccess)
            {
                return Results.Ok();
            }
            else if (result.HasError<NotFoundError>())
            {
                return Results.NotFound(new ErrorData
                {
                    Message = result.Errors.First().Message
                });
            }
            else if (result.HasError<BusinessError>())
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
        }).RequireAuthorization(AppConstraint.ManagerPolicy);
    }
}
