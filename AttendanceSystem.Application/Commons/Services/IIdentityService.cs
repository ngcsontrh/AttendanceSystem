using AttendanceSystem.Domain.Identities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AttendanceSystem.Application.Commons.Services;

/// <summary>
/// Service for identity management - handles authentication and user account operations.
/// </summary>
public interface IIdentityService
{
    Task<AppUser?> GetUserByIdAsync(Guid userId);
    Task<bool> VerifyUserAsync(AppUser user, string password);
    Task<AppUser?> GetUserByNameAsync(string username);
    Task<string?> GetRoleByUserAsync(AppUser user);
    Task<string?> GetRoleByUserIdAsync(Guid userId);
    Task<Guid> CreateUserAsync(string username, string password);
    Task ChangeUserPasswordAsync(string username, string currentPassword, string newPassword);
    Task AssignRoleToUserAsync(Guid userId, string roleName);
}