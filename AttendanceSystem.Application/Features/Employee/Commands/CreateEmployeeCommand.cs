using AttendanceSystem.Application.Commons.Services;
using AttendanceSystem.Application.Features.Employee.DTOs;
using AttendanceSystem.Domain.Entities;
using AttendanceSystem.Domain.Repositories;
using FluentResults;
using Mapster;
using Microsoft.Extensions.Logging;
using AttendanceSystem.Application.Commons.Errors;

namespace AttendanceSystem.Application.Features.Employee.Commands;

public record CreateEmployeeCommand(
    string Code,
    string FullName,
    string Email,
    Gender Gender,
    Guid DepartmentId,
    EmployeeStatus Status = EmployeeStatus.Active,
    Guid? ManagerId = null
);

public class CreateEmployeeCommandHandler
{
    private readonly IEmployeeRepository _employeeRepository;
    private readonly ILogger<CreateEmployeeCommandHandler> _logger;
    private readonly ICurrentUserService _currentUserService;

    public CreateEmployeeCommandHandler(
        IEmployeeRepository employeeRepository,
        ILogger<CreateEmployeeCommandHandler> logger,
        ICurrentUserService currentUserService)
    {
        _employeeRepository = employeeRepository;
        _logger = logger;
        _currentUserService = currentUserService;
    }

    public async Task<Result<EmployeeDTO>> ExecuteAsync(CreateEmployeeCommand command)
    {
        try
        {
            var entity = command.Adapt<Domain.Entities.Employee>();
            entity.Id = Guid.CreateVersion7();
            entity.CreatedAt = DateTime.Now;
            entity.CreatedById = _currentUserService.GetCurrentUserId();

            await _employeeRepository.AddAsync(entity);

            return Result.Ok(entity.Adapt<EmployeeDTO>());
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Có lỗi xảy ra trong quá trình tạo nhân viên");
            return Result.Fail<EmployeeDTO>(new InternalError());
        }
    }
}
