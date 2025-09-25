using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RAttendanceSystem.Application.Services
{
    public interface IIdentityService
    {
        Task<CreateUserResult> CreateUserAsync(CreateUserRequest request);
        Task<AssignRoleResult> AssignRoleAsync(AssignRoleRequest request);
        Task<GetRolesResult> GetRolesAsync();
        Task<GetRoleByNameResult> GetRoleByNameAsync(GetRoleByNameRequest request);
    }

    public class IdentityResult
    {
        public bool Succeeded { get; set; }
        public IReadOnlyList<string> Errors { get; set; } = Array.Empty<string>();
    }

    public class CreateUserRequest
    {
        public required string Username { get; set; }
        public required string Email { get; set; }
        public required string Password { get; set; }
        public required string EmployeeId { get; set; }
    }

    public class CreateUserResult : IdentityResult
    {
        public string? UserId { get; set; }
    }

    public class AssignRoleRequest
    {
        public required string UserId { get; set; }
        public required string RoleId { get; set; }
        public required string RoleName { get; set; }
    }

    public class AssignRoleResult : IdentityResult
    {
    }

    public class GetRolesResult : IdentityResult
    {
        public IReadOnlyList<RoleRepresentation> Roles { get; set; } = Array.Empty<RoleRepresentation>();
    }

    public class GetRoleByNameRequest
    {
        public required string RoleName { get; set; }
    }

    public class GetRoleByNameResult : IdentityResult
    {
        public RoleRepresentation? Role { get; set; }
    }

    public class RoleRepresentation
    {
        public string Id { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public bool? Composite { get; set; }
        public bool? ClientRole { get; set; }
        public string? ContainerId { get; set; }
    }
}