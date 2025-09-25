using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RAttendanceSystem.Application.UseCases.DepartmentUC.Queries
{
    public class GetListDepartmentQueryHandler
    {
        private readonly IDepartmentRepository _departmentRepository;
        private readonly ILogger<GetListDepartmentQueryHandler> _logger;

        public GetListDepartmentQueryHandler(
            IDepartmentRepository departmentRepository,
            ILogger<GetListDepartmentQueryHandler> logger)
        {
            _departmentRepository = departmentRepository;
            _logger = logger;
        }

        public async Task<GetListDepartmentResponse> Handle(GetListDepartmentQuery model)
        {
            var departments = await _departmentRepository.GetListAsync(x => true);
            var response = MapToResponse(departments);
            _logger.LogInformation("Retrieved {Count} departments", departments.Count);
            return response;
        }

        private GetListDepartmentResponse MapToResponse(IReadOnlyList<Department> departments)
        {
            var data = departments.Select(x => new GetListDepartmentResponseData(
                x.Id,
                x.Name,
                x.Description)).ToList();
            return new GetListDepartmentResponse(data);
        }
    }
}
