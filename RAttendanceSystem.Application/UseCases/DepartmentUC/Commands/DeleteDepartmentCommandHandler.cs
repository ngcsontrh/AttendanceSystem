using System;
using System.Threading.Tasks;

namespace RAttendanceSystem.Application.UseCases.DepartmentUC.Commands
{
    public class DeleteDepartmentCommandHandler
    {
        private readonly IDepartmentRepository _departmentRepository;
        private readonly ILogger<DeleteDepartmentCommandHandler> _logger;

        public DeleteDepartmentCommandHandler(IDepartmentRepository departmentRepository, ILogger<DeleteDepartmentCommandHandler> logger)
        {
            _departmentRepository = departmentRepository;
            _logger = logger;
        }

        public async Task HandleAsync(DeleteDepartmentCommand command)
        {
            try
            {
                var department = await _departmentRepository.GetRecordAsync(command.Id);
                if (department == null)
                {
                    _logger.LogWarning("Department with ID {DepartmentId} not found.", command.Id);
                    throw new RecordNotFoundException($"Department with ID {command.Id} not found.");
                }
                _departmentRepository.Delete(department);
                await _departmentRepository.SaveChangesAsync();
                _logger.LogInformation("Department with ID {DepartmentId} deleted successfully.", command.Id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting Department with ID {DepartmentId}.", command.Id);
                throw;
            }
        }
    }
}
