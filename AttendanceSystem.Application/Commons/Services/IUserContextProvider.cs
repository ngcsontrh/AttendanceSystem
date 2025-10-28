using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AttendanceSystem.Application.Commons.Services;

/// <summary>
/// Abstraction layer for getting user context information.
/// This isolates the CurrentUserService from HTTP-specific concerns.
/// </summary>
public interface IUserContextProvider
{
    /// <summary>
    /// Gets the current user's claims principal.
    /// </summary>
    /// <returns>The ClaimsPrincipal if user is authenticated, null otherwise</returns>
    System.Security.Claims.ClaimsPrincipal? GetCurrentUser();
}
