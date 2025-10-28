using AttendanceSystem.Application.Commons.Services;
using AttendanceSystem.Domain.Entities;
using AttendanceSystem.Domain.Repositories;
using FluentResults;
using Mapster;
using Microsoft.Extensions.Logging;
using AttendanceSystem.Application.Commons.Errors;


namespace AttendanceSystem.Application.Features.Employee.Commands;

public record UpdateEmployeeCommand(
    Guid Id,
    string Code,
    string FullName,
    string Email,
    Gender Gender,
    Guid DepartmentId,
    EmployeeStatus Status,
    Guid? ManagerId
);

public class UpdateEmployeeCommandHandler
{
    private readonly IEmployeeRepository _employeeRepository;
    private readonly ILogger<UpdateEmployeeCommandHandler> _logger;
    private readonly ICurrentUserService _currentUserService;

    public UpdateEmployeeCommandHandler(
        IEmployeeRepository employeeRepository,
        ILogger<UpdateEmployeeCommandHandler> logger,
        ICurrentUserService currentUserService)
    {
        _employeeRepository = employeeRepository;
        _logger = logger;
        _currentUserService = currentUserService;
    }

    public async Task<Result> ExecuteAsync(UpdateEmployeeCommand command)
    {
        try
        {
            var entity = await _employeeRepository.GetByIdAsync(command.Id);
            if (entity == null)
            {
                return Result.Fail(new NotFoundError());
            }

            command.Adapt(entity);
            entity.UpdatedAt = DateTime.Now;
            entity.UpdatedById = _currentUserService.GetCurrentUserId();

            await _employeeRepository.SaveChangesAsync();
            return Result.Ok();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Có lỗi xảy ra trong quá trình cập nhật nhân viên");
            return Result.Fail(new InternalError());
        }
    }
}
