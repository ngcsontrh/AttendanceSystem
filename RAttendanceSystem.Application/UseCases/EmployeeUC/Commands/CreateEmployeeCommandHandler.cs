using RAttendanceSystem.Application.Services;
using RAttendanceSystem.Domain;
using RAttendanceSystem.Domain.Entities;
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
        private readonly IIdentityService _identityService;
        private readonly ILogger<CreateEmployeeCommandHandler> _logger;

        public CreateEmployeeCommandHandler(IEmployeeRepository employeeRepository,
            IIdentityService identityService,
            ILogger<CreateEmployeeCommandHandler> logger)
        {
            _employeeRepository = employeeRepository;
            _identityService = identityService;
            _logger = logger;
        }

        public async Task<CreateEmployeeResponse> HandleAsync(CreateEmployeeCommand model)
        {
            try
            {
                var isExist = await _employeeRepository.AnyAsync(x => x.Code == model.Code);
                if (isExist)
                {
                    throw new Exception("Employee code already exists");
                }
                var entity = new Employee { Id = Guid.CreateVersion7() };
                var createIdentityUserResult = await _identityService.CreateUserAsync(new CreateUserRequest
                {
                    Username = model.Code,
                    Email = model.Email,
                    Password = AppConstraint.DefaultPassword,
                    EmployeeId = entity.Id.ToString()
                });
                if (!createIdentityUserResult.Succeeded)
                {
                    throw new Exception(createIdentityUserResult.Errors.FirstOrDefault() ?? "Failed to create user in identity service");
                }
                entity.KeycloakId = createIdentityUserResult.UserId;
                MapToEntity(entity, model);

                _employeeRepository.Add(entity);
                await _employeeRepository.SaveChangesAsync();

                _logger.LogInformation("Employee {EmployeeId} is created", entity.Id);

                return new CreateEmployeeResponse(entity.Id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating employee");
                throw;
            }
        }

        private void MapToEntity(Employee entity, CreateEmployeeCommand model)
        {
            entity.BirthDate = model.BirthDate;
            entity.Code = model.Code;
            entity.Email = model.Email;
            entity.TitleId = model.TitleId;
            entity.DepartmentId = model.DepartmentId;
            entity.CreatedAt = DateTime.UtcNow;
            entity.FullName = model.FullName;
            entity.Gender = model.Gender;
        }
    }
}
