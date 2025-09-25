using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RAttendanceSystem.Application.UseCases.EmployeeUC.Queries
{
    public class GetListEmployeeBasicInfoQueryHandler
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly ILogger<GetListEmployeeBasicInfoQueryHandler> _logger;

        public GetListEmployeeBasicInfoQueryHandler(IEmployeeRepository employeeRepository,
            ILogger<GetListEmployeeBasicInfoQueryHandler> logger)
        {
            _employeeRepository = employeeRepository;
            _logger = logger;
        }

        public async Task<GetListEmployeeBasicInfoResponse> HandleAsync(GetListEmployeeBasicInfoQuery query)
        {
            try
            {
                var employees = await _employeeRepository.GetListBasicInfoAsync();
                return MapToResponse(employees);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting list of employee basic info.");
                throw;
            }
        }

        private GetListEmployeeBasicInfoResponse MapToResponse(IReadOnlyList<Employee> employees)
        {
            var data = employees.Select(x => new GetListEmployeeBasicInfoResponseData(
                x.Id,
                x.FullName,
                x.Code)).ToList();
            return new GetListEmployeeBasicInfoResponse(data);
        }
    }
}
