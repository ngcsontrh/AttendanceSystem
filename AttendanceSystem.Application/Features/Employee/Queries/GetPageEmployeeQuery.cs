using AttendanceSystem.Application.Commons.Models;
using AttendanceSystem.Application.Features.Employee.DTOs;
using AttendanceSystem.Domain.Repositories;
using FluentResults;
using Mapster;
using Microsoft.Extensions.Logging;
using AttendanceSystem.Application.Commons.Errors;
using AttendanceSystem.Domain.Specifications;

namespace AttendanceSystem.Application.Features.Employee.Queries;

public record GetPageEmployeeQuery(
    int PageIndex = 1,
    int PageSize = 20,
    string? Code = null,
    string? FullName = null,
    string? Email = null,
    Guid? DepartmentId = null
);

public class GetPageEmployeeQueryHandler
{
    private readonly IEmployeeRepository _employeeRepository;
    private readonly ILogger<GetPageEmployeeQueryHandler> _logger;

    public GetPageEmployeeQueryHandler(
        IEmployeeRepository employeeRepository,
        ILogger<GetPageEmployeeQueryHandler> logger)
    {
        _employeeRepository = employeeRepository;
        _logger = logger;
    }

    public async Task<Result<PageData<EmployeeDTO>>> ExecuteAsync(GetPageEmployeeQuery request)
    {
        try
        {
            var specification = new GetPageEmployeeSpecification(request.Code, request.FullName, request.Email, request.DepartmentId);
            var (items, totalCount) = await _employeeRepository.GetPageAsync(specification, request.PageIndex, request.PageSize);
            return Result.Ok(new PageData<EmployeeDTO>
            {
                Items = items.Adapt<List<EmployeeDTO>>(),
                TotalCount = totalCount
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Có lỗi xảy ra trong quá trình lấy danh sách nhân viên");
            return Result.Fail<PageData<EmployeeDTO>>(new InternalError());
        }
    }
}
