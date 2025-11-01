using AttendanceSystem.Application.Commons.Errors;
using AttendanceSystem.Application.Commons.Services;
using AttendanceSystem.Application.Features.User.DTOs;
using FluentResults;
using Mapster;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AttendanceSystem.Application.Features.User.Queries;
public record GetCurrentUserQuery(
    Guid UserId
);

public class GetCurrentUserQueryHandler
{
    private readonly ILogger<GetCurrentUserQueryHandler> _logger;
    private readonly IIdentityService _identityService;
    
    public GetCurrentUserQueryHandler(
        ILogger<GetCurrentUserQueryHandler> logger,
        IIdentityService identityService
        )
    {
        _logger = logger;
        _identityService = identityService;
    }

    public async Task<Result<AppUserDTO>> ExecuteAsync(GetCurrentUserQuery query)
    {
        try
        {
            var user = await _identityService.GetUserByIdAsync(query.UserId);
            if (user == null)
            {
                _logger.LogWarning("Không tìm thấy người dùng với Id: {UserId}", query.UserId);
                return Result.Fail<AppUserDTO>(new NotFoundError("Người dùng không tồn tại"));
            }
            return Result.Ok(user.Adapt<AppUserDTO>());
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Có lỗi xảy ra trong quá trình lấy thông tin người dùng hiện tại");
            return Result.Fail<AppUserDTO>(new InternalError());
        }
    }
}