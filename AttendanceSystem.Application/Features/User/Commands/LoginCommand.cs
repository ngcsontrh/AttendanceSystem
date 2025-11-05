using FluentResults;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using AttendanceSystem.Application.Commons.Errors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using AttendanceSystem.Domain.Repositories;
using AttendanceSystem.Domain.Identities;
using AttendanceSystem.Application.Commons;
using AttendanceSystem.Application.Commons.Services;

namespace AttendanceSystem.Application.Features.User.Command;
public record LoginCommand(
    string Username,
    string Password
);

public record LoginCommandResponse(
    string Token,
    DateTime Expiration
);

public class LoginCommandHandler
{ 
    private readonly IIdentityService _identityService;
    private readonly IEmployeeRepository _employeeRepository;
    private readonly ILogger<LoginCommandHandler> _logger;
    private readonly IConfiguration _configuration;

    public LoginCommandHandler(
        IIdentityService identityService,
        ILogger<LoginCommandHandler> logger,
        IEmployeeRepository employeeRepository,
        IConfiguration configuration
        )
    {
        _identityService = identityService;
        _employeeRepository = employeeRepository;
        _logger = logger;
        _configuration = configuration;
    }

    public async Task<Result<LoginCommandResponse>> ExecuteAsync(LoginCommand command)
    {
        try
        {
            var user = await _identityService.GetUserByNameAsync(command.Username);
            if (user == null)
            {
                return Result.Fail<LoginCommandResponse>(new BusinessError("Người dùng không tồn tại"));
            }
            var isValid = await _identityService.VerifyUserAsync(user, command.Password);
            if (!isValid)
            {
                return Result.Fail<LoginCommandResponse>(new BusinessError("Mật khẩu không khớp"));
            }
            var role = await _identityService.GetRoleByUserAsync(user);
            var employee = await _employeeRepository.GetByUserIdAsync(user.Id);
            var (token, expiration) = GenerateJwtToken(user, role, employee);
            var response = new LoginCommandResponse(
                Token: token,
                Expiration: expiration
            );
            return Result.Ok(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Có lỗi xảy ra trong quá trình đăng nhập");
            return Result.Fail<LoginCommandResponse>(new InternalError());
        }
    }

    private (string, DateTime) GenerateJwtToken(
        AppUser user,
        string? role,
        Domain.Entities.Employee? employee)
    {
        var jwtSettings = _configuration.GetSection("JwtSettings");
        var secretKey = jwtSettings["Secret"] ?? throw new InvalidOperationException("JWT Secret key not configured");
        var issuer = jwtSettings["Issuer"] ?? throw new InvalidOperationException("JWT Issuer not configured");
        var audience = jwtSettings["Audience"] ?? throw new InvalidOperationException("JWT Audience not configured");
        var expirationHours = int.TryParse(jwtSettings["ExpirationHours"], out var hours) ? hours : 24;

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.UserName ?? string.Empty),
            new Claim(ClaimTypes.Email, user.Email ?? string.Empty),
        };

        if (!string.IsNullOrEmpty(role))
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }

        if (employee != null)
        {
            claims.Add(new Claim(AppConstraint.EmployeeIdentifier, employee.Id.ToString()));
            claims.Add(new Claim(AppConstraint.EmployeeName, employee.FullName));
        }

        var token = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: claims,
            expires: DateTime.UtcNow.AddHours(expirationHours),
            signingCredentials: creds
        );

        var tokenHandler = new JwtSecurityTokenHandler();
        var encodedToken = tokenHandler.WriteToken(token);
        var expiration = DateTime.UtcNow.AddHours(expirationHours);

        return (encodedToken, expiration);
    }
}
