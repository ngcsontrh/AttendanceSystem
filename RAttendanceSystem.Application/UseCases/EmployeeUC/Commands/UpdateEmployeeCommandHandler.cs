using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RAttendanceSystem.Application.UseCases.EmployeeUC.Commands
{
    public class UpdateEmployeeCommandHandler
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly ILogger<UpdateEmployeeCommandHandler> _logger;
        public UpdateEmployeeCommandHandler(IEmployeeRepository employeeRepository,
            ILogger<UpdateEmployeeCommandHandler> logger)
        {
            _employeeRepository = employeeRepository;
            _logger = logger;
        }
        public async Task HandleAsync(UpdateEmployeeCommand model)
        {
            try
            {
                var entity = await _employeeRepository.GetRecordAsync(model.Id);
                if (entity == null)
                {
                    _logger.LogWarning("Employee {EmployeeId} not found.", model.Id);
                    throw new RecordNotFoundException($"Employee with ID {model.Id} not found.");
                }
                entity.FullName = model.FullName;
                entity.Code = model.Code;
                entity.Gender = model.Gender;
                entity.BirthDate = model.BirthDate;
                entity.DepartmentId = model.DepartmentId;
                entity.TitleId = model.TitleId;
                await _employeeRepository.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating employee {EmployeeId}.", model.Id);
                throw;
            }
        }
    }
}
