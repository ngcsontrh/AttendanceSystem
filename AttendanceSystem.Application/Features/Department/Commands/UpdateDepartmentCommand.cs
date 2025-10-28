using AttendanceSystem.Application.Commons.Services;
using AttendanceSystem.Domain.Repositories;
using FluentResults;
using Mapster;
using Microsoft.Extensions.Logging;
using AttendanceSystem.Application.Commons;
using AttendanceSystem.Application.Commons.Errors;

using AttendanceSystem.Application.Features.Department.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AttendanceSystem.Application.Features.Department.Commands;
public record UpdateDepartmentCommand(
    Guid Id,
    string Code,
    string Name,
    string? Description
);

public class UpdateDepartmentCommandHandler
{
    private readonly IDepartmentRepository _departmentRepository;
    private readonly ILogger<UpdateDepartmentCommandHandler> _logger;
    private readonly ICurrentUserService _currentUserService;

    public UpdateDepartmentCommandHandler(
        IDepartmentRepository departmentRepository,
        ILogger<UpdateDepartmentCommandHandler> logger,
        ICurrentUserService currentUserService)
    {
        _departmentRepository = departmentRepository;
        _logger = logger;
        _currentUserService = currentUserService;
    }

    public async Task<Result> ExecuteAsync(UpdateDepartmentCommand command)
    {
        try
        {
            var entity = await _departmentRepository.GetByIdAsync(command.Id);
            if (entity == null)
            {
                return Result.Fail(new NotFoundError());
            }
            command.Adapt(entity);
            entity.UpdatedAt = DateTime.Now;
            entity.UpdatedById = _currentUserService.GetCurrentUserId();
            await _departmentRepository.SaveChangesAsync();
            return Result.Ok();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Có lỗi xảy ra trong quá trình cập nhật phòng ban");
            return Result.Fail(new InternalError());
        }
    }
}