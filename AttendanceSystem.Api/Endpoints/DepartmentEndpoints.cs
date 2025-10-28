using AttendanceSystem.Api.Commons;
using AttendanceSystem.Application.Commons;
using AttendanceSystem.Application.Commons.Errors;
using AttendanceSystem.Application.Features.Department.Commands;
using AttendanceSystem.Application.Features.Department.Queries;
using Microsoft.AspNetCore.Mvc;
using AttendanceSystem.Application.Commons.Errors;
using AttendanceSystem.Application.Commons.Models;

namespace AttendanceSystem.Api.Endpoints;

public class DepartmentEndpoints : IEndpointRegister
{
    public void MapRoutes(IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/departments")
            .WithTags("Departments")
            .RequireAuthorization();

        group.MapGet("/", async (
            [AsParameters] GetPageDepartmentQuery request,
            [FromServices] GetPageDepartmentQueryHandler handler) =>
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
            [FromServices] GetDepartmentByIdQueryHandler handler) =>
        {
            var result = await handler.ExecuteAsync(new GetDepartmentByIdQuery(id));
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
            [FromBody] CreateDepartmentCommand request,
            [FromServices] CreateDepartmentCommandHandler handler
            ) =>
        {
            var result = await handler.ExecuteAsync(request);
            if (result.IsSuccess)
            {
                return Results.Created($"/api/departments/{result.Value.Id}", result.Value);
            }
            return Results.InternalServerError(new ErrorData
            {
                Message = result.Errors.First().Message
            });
        }).RequireAuthorization(AppConstraint.AdminPolicy);

        group.MapPut("/{id}", async (
            [FromRoute] Guid id,
            [FromBody] UpdateDepartmentCommand request,
            [FromServices] UpdateDepartmentCommandHandler handler) =>
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
            [FromServices] DeleteDepartmentCommandHandler handler) =>
        {
            var result = await handler.ExecuteAsync(new DeleteDepartmentCommand(id));
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
