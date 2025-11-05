using AttendanceSystem.Application.Commons.Services;
using AttendanceSystem.Domain.Repositories;
using FluentResults;
using Microsoft.Extensions.Logging;
using AttendanceSystem.Application.Commons.Errors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AttendanceSystem.Domain.Events.User;

namespace AttendanceSystem.Application.Features.User.Command;
public record CreateUserCommand(
    string UserName,
    string Password,
    Guid EmployeeId);

public class CreateUserCommandHandler
{
    private readonly IIdentityService _identityService;
    private readonly IEmployeeRepository _employeeRepository;
    private readonly ILogger<CreateUserCommandHandler> _logger;
    private readonly IMessagingService _messagingService;

    public CreateUserCommandHandler(
        IIdentityService identityService,
        IEmployeeRepository employeeRepository,
        ILogger<CreateUserCommandHandler> logger,
        IMessagingService messagingService)
    {
        _identityService = identityService;
        _employeeRepository = employeeRepository;
        _logger = logger;
        _messagingService = messagingService;
    }

    public async Task<Result<Guid>> ExecuteAsync(CreateUserCommand command)
    {
        try
        {
            var employee = await _employeeRepository.GetByIdAsync(command.EmployeeId);
            if (employee == null)
            {
                _logger.LogWarning("Không tìm thấy nhân viên với Id: {EmployeeId}", command.EmployeeId);
                return Result.Fail<Guid>(new BusinessError("Nhân viên không tồn tại"));
            }
            var entityId = await _identityService.CreateUserAsync(command.UserName, command.Password);
            await _employeeRepository.UpdateUserIdAsync(employee, entityId);
            await _messagingService.PublishAsync(new UserCreatedEvent(entityId, command.UserName, employee.Email));
            return Result.Ok(entityId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Có lỗi xảy ra trong quá trình tạo người dùng");
            return Result.Fail<Guid>(new InternalError());
        }
    }
}