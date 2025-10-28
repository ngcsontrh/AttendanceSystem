using AttendanceSystem.Domain.Repositories;
using FluentResults;
using Microsoft.Extensions.Logging;
using AttendanceSystem.Application.Commons.Errors;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AttendanceSystem.Application.Features.Department.Commands;
public record DeleteDepartmentCommand(
    Guid Id
);

public class DeleteDepartmentCommandHandler
{
    private readonly IEmployeeRepository _employeeRepository;
    private readonly IDepartmentRepository _departmentRepository;
    private readonly ILogger<DeleteDepartmentCommandHandler> _logger;

    public DeleteDepartmentCommandHandler(
        IEmployeeRepository employeeRepository,
        IDepartmentRepository departmentRepository,
        ILogger<DeleteDepartmentCommandHandler> logger)
    {
        _employeeRepository = employeeRepository;
        _departmentRepository = departmentRepository;
        _logger = logger;
    }

    public async Task<Result> ExecuteAsync(DeleteDepartmentCommand command)
    {
        try
        {
            var hasEmployees = await _employeeRepository.CheckEmployeeExistsByDepartmentIdAsync(command.Id);
            if (hasEmployees)
            {
                return Result.Fail(new BusinessError("Không thể xóa phòng ban vì có nhân viên thuộc phòng ban này"));
            }
            var entity = await _departmentRepository.GetByIdAsync(command.Id);
            if (entity == null)
            {
                return Result.Fail(new NotFoundError("Phòng ban không tồn tại"));
            }
            await _departmentRepository.DeleteAsync(entity);
            return Result.Ok();
        }
        catch (Exception ex)
        {            
            _logger.LogError(ex, "Có lỗi xảy ra trong quá trình xóa phòng ban");
            return Result.Fail(new InternalError());
        }
    }
}