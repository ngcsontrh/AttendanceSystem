using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AttendanceSystem.Application.Commons.Services;

/// <summary>
/// Service for retrieving current user information.
/// Depends only on IUserContextProvider abstraction, not on HTTP context directly.
/// </summary>
public interface ICurrentUserService
{
    /// <summary>
    /// Gets the current employee ID from user claims.
    /// </summary>
    /// <returns>The current employee ID</returns>
    /// <exception cref="InvalidOperationException">When user is not authenticated or claim is missing</exception>
    Guid GetCurrentEmployeeId();

    /// <summary>
    /// Gets the current user ID (from ASP.NET Core Identity).
    /// </summary>
    /// <returns>The current user ID</returns>
    /// <exception cref="InvalidOperationException">When user is not authenticated or claim is missing</exception>
    Guid GetCurrentUserId();

    /// <summary>
    /// Gets all roles assigned to the current user.
    /// </summary>
    /// <returns>List of role names</returns>
    /// <exception cref="InvalidOperationException">When user is not authenticated</exception>
    List<string> GetCurrentUserRoles();
}
