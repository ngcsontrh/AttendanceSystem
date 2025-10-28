using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AttendanceSystem.Application.Commons.Services;
using Microsoft.AspNetCore.Http;

namespace AttendanceSystem.Infrastructure.Services;

/// <summary>
/// HTTP-based implementation of IUserContextProvider.
/// This is the only place in the codebase that directly depends on IHttpContextAccessor.
/// </summary>
public class HttpUserContextProvider : IUserContextProvider
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public HttpUserContextProvider(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public ClaimsPrincipal? GetCurrentUser()
    {
        return _httpContextAccessor.HttpContext?.User;
    }
}
