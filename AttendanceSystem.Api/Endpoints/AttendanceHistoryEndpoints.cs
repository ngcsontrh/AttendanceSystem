using AttendanceSystem.Api.Commons;
using AttendanceSystem.Application.Commons;
using AttendanceSystem.Application.Commons.Errors;
using AttendanceSystem.Application.Features.AttendanceHistory.Commands;
using AttendanceSystem.Application.Features.AttendanceHistory.Queries;
using Microsoft.AspNetCore.Mvc;
using AttendanceSystem.Application.Commons.Errors;
using AttendanceSystem.Application.Commons.Models;

namespace AttendanceSystem.Api.Endpoints;

public class AttendanceHistoryEndpoints : IEndpointRegister
{
    public void MapRoutes(IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/attendance-histories")
            .WithTags("AttendanceHistories")
            .RequireAuthorization();

        group.MapGet("/", async (
            [AsParameters] GetPageAttendanceHistoryQuery request,
            [FromServices] GetPageAttendanceHistoryQueryHandler handler) =>
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
            [FromServices] GetAttendanceHistoryByIdQueryHandler handler) =>
        {
            var result = await handler.ExecuteAsync(new GetAttendanceHistoryByIdQuery(id));
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
            [FromBody] CreateAttendanceHistoryCommand request,
            [FromServices] CreateAttendanceHistoryCommandHandler handler
            ) =>
        {
            var result = await handler.ExecuteAsync(request);
            if (result.IsSuccess)
            {
                return Results.Created($"/api/attendance-histories/{result.Value.Id}", result.Value);
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
        }).RequireAuthorization(AppConstraint.StaffPolicy);
    }
}
