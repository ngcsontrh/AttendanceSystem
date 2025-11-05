using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using AttendanceSystem.Application.Commons;
using AttendanceSystem.Domain.Identities;
using AttendanceSystem.Application.Commons.Services;

namespace AttendanceSystem.Infrastructure.Services;

public class IdentityService : IIdentityService
{
    private readonly UserManager<AppUser> _userManager;
    private readonly IRoleStore<AppRole> _roleStore;
    private readonly IUserStore<AppUser> _userStore;

    public IdentityService(UserManager<AppUser> userManager, IRoleStore<AppRole> roleStore, IUserStore<AppUser> userStore)
    {
        _userManager = userManager;
        _roleStore = roleStore;
        _userStore = userStore;
    }

    public async Task<bool> VerifyUserAsync(AppUser user, string password)
    {        
        var isPasswordValid = await _userManager.CheckPasswordAsync(user, password);
        return isPasswordValid;
    }

    public async Task<Guid> CreateUserAsync(string username, string password)
    {
        var entity = new AppUser
        {
            Id = Guid.CreateVersion7(),
            Email = username,
            NormalizedEmail = username.ToUpper(),
            UserName = username,
            NormalizedUserName = username.ToUpper(),
            EmailConfirmed = false,
            CreatedAt = DateTime.Now
        };
        var result = await _userManager.CreateAsync(entity, password);

        if (!result.Succeeded)
        {
            var errors = string.Join(", ", result.Errors.Select(e => e.Description));
            throw new InvalidOperationException($"Registration failed: {errors}");
        }
        return entity.Id;
    }

    public async Task ChangeUserPasswordAsync(string username, string currentPassword, string newPassword)
    {
        var user = await _userManager.FindByNameAsync(username);
        if (user == null)
        {
            throw new InvalidOperationException("User not found");
        }

        var isPasswordValid = await _userManager.CheckPasswordAsync(user, currentPassword);
        if (!isPasswordValid)
        {
            throw new UnauthorizedAccessException("Current password is incorrect");
        }

        var result = await _userManager.ChangePasswordAsync(user, currentPassword, newPassword);
        if (!result.Succeeded)
        {
            var errors = string.Join(", ", result.Errors.Select(e => e.Description));
            throw new InvalidOperationException($"Password change failed: {errors}");
        }
    }

    public Task<AppUser?> GetUserByNameAsync(string username)
    {
        var user = _userManager.FindByNameAsync(username);
        return user;
    }

    public async Task AssignRoleToUserAsync(Guid userId, string roleName)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());
        if (user == null)
        {
            throw new InvalidOperationException("User not found");
        }
        var result = await _userManager.AddToRoleAsync(user, roleName);
        if (!result.Succeeded)
        {
            var errors = string.Join(", ", result.Errors.Select(e => e.Description));
            throw new InvalidOperationException($"Assigning role failed: {errors}");
        }
    }

    public async Task<string?> GetRoleByUserAsync(AppUser user)
    {
        var roles = await _userManager.GetRolesAsync(user);
        return roles.Count > 0 ? roles.First() : null;
    }

    public async Task<AppUser?> GetUserByIdAsync(Guid userId)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());
        return user;
    }

    public async Task<string?> GetRoleByUserIdAsync(Guid userId)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());
        if (user == null)
        {
            throw new InvalidOperationException("User not found");
        }
        var role = await GetRoleByUserAsync(user);
        return role;
    }
}

