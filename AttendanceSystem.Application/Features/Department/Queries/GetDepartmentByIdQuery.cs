using AttendanceSystem.Application.Features.Department.DTOs;
using AttendanceSystem.Domain.Repositories;
using FluentResults;
using Mapster;
using Microsoft.Extensions.Logging;
using AttendanceSystem.Application.Commons.Errors;

using AttendanceSystem.Domain.Specifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AttendanceSystem.Application.Features.Department.Queries;
public record GetDepartmentByIdQuery(
    Guid Id
);

public class GetDepartmentByIdQueryHandler
{
    private readonly IDepartmentRepository _departmentRepository;
    private readonly ILogger<GetDepartmentByIdQueryHandler> _logger;

    public GetDepartmentByIdQueryHandler(
        IDepartmentRepository departmentRepository,
        ILogger<GetDepartmentByIdQueryHandler> logger)
    {
        _departmentRepository = departmentRepository;
        _logger = logger;
    }

    public async Task<Result<DepartmentDTO>> ExecuteAsync(GetDepartmentByIdQuery query)
    {
        try
        {
            var department = await _departmentRepository.GetByIdAsync(query.Id);
            if (department == null)
            {
                return Result.Fail<DepartmentDTO>(new NotFoundError());
            }
            return Result.Ok(department.Adapt<DepartmentDTO>());
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Có lỗi xảy ra trong quá trình lấy thông tin phòng ban");
            return Result.Fail<DepartmentDTO>(new InternalError());
        }
    }
}