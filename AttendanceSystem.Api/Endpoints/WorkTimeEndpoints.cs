using AttendanceSystem.Api.Commons;
using AttendanceSystem.Application.Commons;
using AttendanceSystem.Application.Commons.Errors;
using AttendanceSystem.Application.Features.WorkTime.Commands;
using AttendanceSystem.Application.Features.WorkTime.Queries;
using Microsoft.AspNetCore.Mvc;
using AttendanceSystem.Application.Commons.Errors;
using AttendanceSystem.Application.Commons.Models;

namespace AttendanceSystem.Api.Endpoints;

public class WorkTimeEndpoints : IEndpointRegister
{
    public void MapRoutes(IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/work-times")
            .WithTags("WorkTimes")
            .RequireAuthorization();

        group.MapGet("/", async (
            [AsParameters] GetPageWorkTimeQuery request,
            [FromServices] GetPageWorkTimeQueryHandler handler) =>
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
            [FromServices] GetWorkTimeByIdQueryHandler handler) =>
        {
            var result = await handler.ExecuteAsync(new GetWorkTimeByIdQuery(id));
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
            [FromBody] CreateWorkTimeCommand request,
            [FromServices] CreateWorkTimeCommandHandler handler
            ) =>
        {
            var result = await handler.ExecuteAsync(request);
            if (result.IsSuccess)
            {
                return Results.Created($"/api/work-times/{result.Value.Id}", result.Value);
            }
            return Results.InternalServerError(new ErrorData
            {
                Message = result.Errors.First().Message
            });
        }).RequireAuthorization(AppConstraint.AdminPolicy);

        group.MapPut("/{id}", async (
            [FromRoute] Guid id,
            [FromBody] UpdateWorkTimeCommand request,
            [FromServices] UpdateWorkTimeCommandHandler handler) =>
        {
            var command = request with { Id = id };
            var result = await handler.ExecuteAsync(command);
            if (result.IsSuccess)
            {
                return Results.NoContent();
            }
            return Results.InternalServerError(new ErrorData
            {
                Message = result.Errors.First().Message
            });
        }).RequireAuthorization(AppConstraint.AdminPolicy);

        group.MapDelete("/{id}", async (
            [FromRoute] Guid id,
            [FromServices] DeleteWorkTimeCommandHandler handler) =>
        {
            var result = await handler.ExecuteAsync(new DeleteWorkTimeCommand(id));
            if (result.IsSuccess)
            {
                return Results.NoContent();
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
        }).RequireAuthorization(AppConstraint.AdminPolicy);
    }
}
