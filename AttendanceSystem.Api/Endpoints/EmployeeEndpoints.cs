using AttendanceSystem.Api.Commons;
using AttendanceSystem.Application.Commons;
using AttendanceSystem.Application.Commons.Errors;
using AttendanceSystem.Application.Features.Employee.Commands;
using AttendanceSystem.Application.Features.Employee.Queries;
using Microsoft.AspNetCore.Mvc;
using AttendanceSystem.Application.Commons.Errors;
using AttendanceSystem.Application.Commons.Models;

namespace AttendanceSystem.Api.Endpoints;

public class EmployeeEndpoints : IEndpointRegister
{
    public void MapRoutes(IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/employees")
            .WithTags("Employees")
            .RequireAuthorization();

        group.MapGet("/", async (
            [AsParameters] GetPageEmployeeQuery request,
            [FromServices] GetPageEmployeeQueryHandler handler) =>
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
        }).RequireAuthorization(AppConstraint.ManagerPolicy);

        group.MapGet("/{id}", async (
            [FromRoute] Guid id,
            [FromServices] GetEmployeeByIdQueryHandler handler) =>
        {
            var result = await handler.ExecuteAsync(new GetEmployeeByIdQuery(id));
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
            [FromBody] CreateEmployeeCommand request,
            [FromServices] CreateEmployeeCommandHandler handler
            ) =>
        {
            var result = await handler.ExecuteAsync(request);
            if (result.IsSuccess)
            {
                return Results.Created($"/api/employees/{result.Value.Id}", result.Value);
            }
            return Results.InternalServerError(new ErrorData
            {
                Message = result.Errors.First().Message
            });
        }).RequireAuthorization(AppConstraint.AdminPolicy);

        group.MapPut("/{id}", async (
            [FromRoute] Guid id,
            [FromBody] UpdateEmployeeCommand request,
            [FromServices] UpdateEmployeeCommandHandler handler) =>
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
    }
}
