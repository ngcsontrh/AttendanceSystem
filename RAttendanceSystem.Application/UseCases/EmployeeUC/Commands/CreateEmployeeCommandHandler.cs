using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RAttendanceSystem.Application.UseCases.EmployeeUC.Commands
{
    public class CreateEmployeeCommandHandler
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly ILogger<CreateEmployeeCommandHandler> _logger;

        public CreateEmployeeCommandHandler(IEmployeeRepository employeeRepository,
            ILogger<CreateEmployeeCommandHandler> logger)
        {
            _employeeRepository = employeeRepository;
            _logger = logger;
        }

        public async Task<Guid> HandleAsync(CreateEmployeeCommand model)
        {
            try
            {
                var entity = new Employee
                {
                    Id = Guid.CreateVersion7(),
                    BirthDate = model.BirthDate,
                    Code = model.Code,
                    TitleId = model.TitleId,
                    DepartmentId = model.DepartmentId,
                    CreatedAt = DateTime.UtcNow,
                    FullName = model.FullName,
                    Gender = model.Gender,
                };
                _employeeRepository.Add(entity);
                await _employeeRepository.SaveChangesAsync();
                _logger.LogInformation("Employee {EmployeeId} is created.", entity.Id);
                return entity.Id;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating employee.");
                throw;
            }
        }
    }
}
