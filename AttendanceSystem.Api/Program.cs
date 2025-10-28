using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using AttendanceSystem.ServiceDefaults;
using AttendanceSystem.Api;
using AttendanceSystem.Infrastructure;
using AttendanceSystem.Application;
using AttendanceSystem.Migrator;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.Services.AddHttpContextAccessor();

builder.Services.AddControllers();

builder.Services.AddAuthenticationConfiguration(builder.Configuration);

builder.Services.AddAuthorization();

builder.Services.AddOpenApi();

builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddMigratorServices(builder.Configuration);

builder.Services.AddApplication();

builder.Services.AddCorsPolicies();

builder.Services.AddAuthorizationPolicies();

var app = builder.Build();

app.UseCorsPolicies();

app.MapDefaultEndpoints();

app.MapEndpointDefinitions();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.ApplyMigrations();
}

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
