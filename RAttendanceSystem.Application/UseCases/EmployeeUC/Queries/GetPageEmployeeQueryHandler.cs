using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RAttendanceSystem.Application.UseCases.EmployeeUC.Queries
{
    public class GetPageEmployeeQueryHandler
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly ILogger<GetPageEmployeeQueryHandler> _logger;

        public GetPageEmployeeQueryHandler(IEmployeeRepository employeeRepository,
            ILogger<GetPageEmployeeQueryHandler> logger)
        {
            _employeeRepository = employeeRepository;
            _logger = logger;
        }

        public async Task<GetPageEmployeeResponse> HandleAsync(GetPageEmployeeQuery model)
        {
            try
            {
                int skip = (model.PageNumber - 1) * model.PageSize;
                int take = model.PageSize;
                var spec = new EmployeeFilterSpecificationBuilder()
                    .WithFullName(model.FullName)
                    .WithCode(model.Code)
                    .WithDepartmentId(model.DepartmentId)
                    .WithTitleId(model.TitleId)
                    .WithOrderByDescending(x => x.CreatedAt)
                    .WithPaging(skip, take)
                    .Build();

                var result = await _employeeRepository.GetPageAsync(spec);

                return MapToResponse(result.Item1, result.Item2);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting employee's page.");
                throw;
            }
        }
        
        private GetPageEmployeeResponse MapToResponse(IReadOnlyList<Employee> employees, int total)
        {
            var data = employees.Select(x => new GetPageEmployeeResponseData(
                x.Id,
                x.FullName,
                x.Code,
                x.Gender,
                x.BirthDate,
                x.DepartmentId,
                x.TitleId)).ToList();
            return new GetPageEmployeeResponse(data, total);
        }
    }
}
