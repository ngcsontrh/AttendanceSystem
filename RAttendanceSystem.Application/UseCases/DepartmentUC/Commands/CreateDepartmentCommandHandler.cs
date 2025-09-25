using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RAttendanceSystem.Application.UseCases.DepartmentUC.Commands
{
    public class CreateDepartmentCommandHandler
    {
        private readonly IDepartmentRepository _departmentRepository;
        private readonly ILogger<CreateDepartmentCommandHandler> _logger;

        public CreateDepartmentCommandHandler(IDepartmentRepository departmentRepository, ILogger<CreateDepartmentCommandHandler> logger)
        {
            _departmentRepository = departmentRepository;
            _logger = logger;
        }

        public async Task<CreateDepartmentResponse> HandleAsync(CreateDepartmentCommand command)
        {
            try
            {
                var newDepartment = new Department
                {
                    Id = Guid.CreateVersion7(),
                    Name = command.Name,
                    Description = command.Description,
                    CreatedAt = DateTime.UtcNow
                };
                _departmentRepository.Add(newDepartment);
                await _departmentRepository.SaveChangesAsync();
                _logger.LogInformation("Created new department with ID: {DepartmentId}", newDepartment.Id);
                return new CreateDepartmentResponse(newDepartment.Id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating a new department.");
                throw;
            }            
        }
    }
}
