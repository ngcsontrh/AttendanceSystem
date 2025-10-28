using AttendanceSystem.Application.Commons.Models;
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
public record GetPageDepartmentQuery(
   int PageIndex = 1,
   int PageSize = 20,
   string? Code = null,
   string? Name = null
);

public class GetPageDepartmentQueryHandler
{
    private readonly IDepartmentRepository _departmentRepository;
    private readonly ILogger<GetPageDepartmentQueryHandler> _logger;

    public GetPageDepartmentQueryHandler(
        IDepartmentRepository departmentRepository,
        ILogger<GetPageDepartmentQueryHandler> logger)
    {
        _departmentRepository = departmentRepository;
        _logger = logger;
    }

    public async Task<Result<PageData<DepartmentDTO>>> ExecuteAsync(GetPageDepartmentQuery request)
    {
        try
        {
            var specification = new GetPageDepartmentSpecification(request.Code, request.Name);
            var (items, totalCount) = await _departmentRepository.GetPageAsync(specification, request.PageIndex, request.PageSize);
            return Result.Ok(new PageData<DepartmentDTO>
            {
                Items = items.Adapt<List<DepartmentDTO>>(),
                TotalCount = totalCount
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Có lỗi xảy ra trong quá trình lấy danh sách phòng ban");
            return Result.Fail<PageData<DepartmentDTO>>(new InternalError());
        }        
    }
}