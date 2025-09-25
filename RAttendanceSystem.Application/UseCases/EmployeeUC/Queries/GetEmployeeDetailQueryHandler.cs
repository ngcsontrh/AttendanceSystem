using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RAttendanceSystem.Application.UseCases.EmployeeUC.Queries
{
    public class GetEmployeeDetailQueryHandler
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly ILogger<GetEmployeeDetailQueryHandler> _logger;

        public GetEmployeeDetailQueryHandler(IEmployeeRepository employeeRepository,
            ILogger<GetEmployeeDetailQueryHandler> logger)
        {
            _employeeRepository = employeeRepository;
            _logger = logger;
        }

        public async Task<GetEmployeeDetailResponse> HandleAsync(GetEmployeeDetailQuery model)
        {
            try
            {
                var employee = await _employeeRepository.GetRecordAsync(model.Id);
                if (employee == null)
                {
                    throw new RecordNotFoundException($"Employee with ID {model.Id} not found.");
                }
                return MapToResponse(employee);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting employee detail for ID: {EmployeeId}", model.Id);
                throw;
            }
        }

        private GetEmployeeDetailResponse MapToResponse(Employee employee)
        {
            return new GetEmployeeDetailResponse(
                employee.Id,
                employee.Code,
                employee.FullName,
                employee.Gender,
                employee.BirthDate,
                employee.DepartmentId,
                employee.TitleId,
                employee.CreatedAt,
                employee.Title == null ? null : new GetEmployeeDetailResponseTitleData(
                    employee.Title.Id,
                    employee.Title.Name
                ),
                employee.Department == null ? null : new GetEmployeeDetailResponseDepartmentData(
                    employee.Department.Id,
                    employee.Department.Name
                )
            );
        }
    }
}
