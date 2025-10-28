using AttendanceSystem.Application.Commons.Services;
using AttendanceSystem.Application.Features.Department.DTOs;
using AttendanceSystem.Domain.Repositories;
using FluentResults;
using Mapster;
using Microsoft.Extensions.Logging;
using AttendanceSystem.Application.Commons;
using AttendanceSystem.Application.Commons.Errors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace AttendanceSystem.Application.Features.Department.Commands;
public record CreateDepartmentCommand(
    string Code,
    string Name,
    string? Description
);

public class CreateDepartmentCommandHandler
{
    private readonly IDepartmentRepository _departmentRepository;
    private readonly ILogger<CreateDepartmentCommandHandler> _logger;
    private readonly ICurrentUserService _currentUserService;

    public CreateDepartmentCommandHandler(IDepartmentRepository departmentRepository,
        ILogger<CreateDepartmentCommandHandler> logger,
        ICurrentUserService currentUserService)
    {
        _departmentRepository = departmentRepository;
        _logger = logger;
        _currentUserService = currentUserService;
    }

    public async Task<Result<DepartmentDTO>> ExecuteAsync(CreateDepartmentCommand command)
    {
        try
        {
            var employeeId = _currentUserService.GetCurrentEmployeeId();
            var entity = command.Adapt<Domain.Entities.Department>();
            entity.Id = Guid.CreateVersion7();
            entity.CreatedAt = DateTime.Now;            
            entity.CreatedById = _currentUserService.GetCurrentUserId();

            await _departmentRepository.AddAsync(entity);

            return Result.Ok(entity.Adapt<DepartmentDTO>());
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Có lỗi xảy ra trong quá trình xóa phòng ban");
            return Result.Fail<DepartmentDTO>(new InternalError());
        }
    }
}