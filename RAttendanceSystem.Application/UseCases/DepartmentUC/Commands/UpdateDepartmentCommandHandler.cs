using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RAttendanceSystem.Application.UseCases.DepartmentUC.Commands
{
    public class UpdateDepartmentCommandHandler
    {
        private readonly IDepartmentRepository _departmentRepository;
        private readonly ILogger<UpdateDepartmentCommandHandler> _logger;

        public UpdateDepartmentCommandHandler(
            IDepartmentRepository departmentRepository,
            ILogger<UpdateDepartmentCommandHandler> logger)
        {
            _departmentRepository = departmentRepository;
            _logger = logger;
        }

        public async Task HandleAsync(UpdateDepartmentCommand command)
        {
            try
            {
                var department = await _departmentRepository.GetRecordAsync(command.Id);
                if (department == null)
                {
                    _logger.LogWarning("Department with ID {DepartmentId} not found.", command.Id);
                    throw new RecordNotFoundException($"Department with ID {command.Id} not found.");
                }
                MapToEntity(department, command);
                await _departmentRepository.SaveChangesAsync();
                _logger.LogInformation("Department with ID {DepartmentId} updated successfully.", command.Id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating department with ID {DepartmentId}.", command.Id);
                throw;
            }            
        }

        void MapToEntity(Department entity, UpdateDepartmentCommand command)
        {
            entity.Name = command.Name;
            entity.Description = command.Description;
        }
    }
}
