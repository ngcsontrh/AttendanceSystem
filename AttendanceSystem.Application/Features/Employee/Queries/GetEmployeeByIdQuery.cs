using AttendanceSystem.Application.Features.Employee.DTOs;
using AttendanceSystem.Domain.Repositories;
using FluentResults;
using Mapster;
using Microsoft.Extensions.Logging;
using AttendanceSystem.Application.Commons.Errors;


namespace AttendanceSystem.Application.Features.Employee.Queries;

public record GetEmployeeByIdQuery(
    Guid Id
);

public class GetEmployeeByIdQueryHandler
{
    private readonly IEmployeeRepository _employeeRepository;
    private readonly ILogger<GetEmployeeByIdQueryHandler> _logger;

    public GetEmployeeByIdQueryHandler(
        IEmployeeRepository employeeRepository,
        ILogger<GetEmployeeByIdQueryHandler> logger)
    {
        _employeeRepository = employeeRepository;
        _logger = logger;
    }

    public async Task<Result<EmployeeDTO>> ExecuteAsync(GetEmployeeByIdQuery query)
    {
        try
        {
            var employee = await _employeeRepository.GetByIdAsync(query.Id);
            if (employee == null)
            {
                return Result.Fail<EmployeeDTO>(new NotFoundError());
            }

            return Result.Ok(employee.Adapt<EmployeeDTO>());
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Có lỗi xảy ra trong quá trình lấy thông tin nhân viên");
            return Result.Fail<EmployeeDTO>(new InternalError());
        }
    }
}
