using AttendanceSystem.Application.Commons.Services;
using FluentResults;
using Microsoft.Extensions.Logging;
using AttendanceSystem.Application.Commons.Errors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AttendanceSystem.Application.Features.User.Command;
public record AssignRoleCommand(
    Guid UserId,
    string Role
);

public class AssignRoleCommandHandler
{
    private readonly ILogger<AssignRoleCommandHandler> _logger;
    private readonly IIdentityService _identityService;

    public AssignRoleCommandHandler(
        ILogger<AssignRoleCommandHandler> logger,
        IIdentityService identityService)
    {
        _logger = logger;
        _identityService = identityService;
    }

    public async Task<Result> ExecuteAsync(AssignRoleCommand command)
    {
        try
        {
            await _identityService.AssignRoleToUserAsync(command.UserId, command.Role);
            return Result.Ok();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Có lỗi xảy ra trong quá trình gán vai trò cho người dùng");
            return Result.Fail(new InternalError());
        }
    }
}