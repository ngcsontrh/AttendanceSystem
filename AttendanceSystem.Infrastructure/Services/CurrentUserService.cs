using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AttendanceSystem.Application.Commons;
using AttendanceSystem.Application.Commons.Services;

namespace AttendanceSystem.Infrastructure.Services;

/// <summary>
/// Service for retrieving current authenticated user information.
/// Depends on IUserContextProvider abstraction instead of HTTP context directly.
/// This allows easy testing and alternative implementations (e.g., JWT token provider, Message Queue context).
/// </summary>
public class CurrentUserService : ICurrentUserService
{
    private readonly IUserContextProvider _userContextProvider;

    public CurrentUserService(IUserContextProvider userContextProvider)
    {
        _userContextProvider = userContextProvider;
    }

    public Guid GetCurrentEmployeeId()
    {
        var currentUser = _userContextProvider.GetCurrentUser();
        if (currentUser == null)
        {
            throw new InvalidOperationException("No user is currently logged in.");
        }
        
        var employeeIdClaim = currentUser.FindFirst(AppConstraint.EmployeeIdentifier);
        if (employeeIdClaim == null)
        {
            throw new InvalidOperationException("Employee identifier claim not found.");
        }
        
        return Guid.Parse(employeeIdClaim.Value);
    }

    public Guid GetCurrentUserId()
    {
        var currentUser = _userContextProvider.GetCurrentUser();
        if (currentUser == null)
        {
            throw new InvalidOperationException("No user is currently logged in.");
        }
        
        var userIdClaim = currentUser.FindFirst(ClaimTypes.NameIdentifier);
        if (userIdClaim == null)
        {
            throw new InvalidOperationException("User identifier claim not found.");
        }
        
        return Guid.Parse(userIdClaim.Value);
    }

    public List<string> GetCurrentUserRoles()
    {
        var currentUser = _userContextProvider.GetCurrentUser();
        if (currentUser == null)
        {
            throw new InvalidOperationException("No user is currently logged in.");
        }
        
        var roleClaims = currentUser.FindAll(ClaimTypes.Role);
        return roleClaims.Select(c => c.Value).ToList();
    }
}

