using AttendanceSystem.Api;
using AttendanceSystem.Application;
using AttendanceSystem.Infrastructure;
using AttendanceSystem.Migrator;
using AttendanceSystem.ServiceDefaults;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.Services.AddHttpContextAccessor();

builder.Services.AddControllers();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Attendance System API", Version = "v1" });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});

builder.Services.AddAuthenticationConfiguration(builder.Configuration);

builder.Services.AddAuthorization();

//builder.Services.AddOpenApi();

builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddMigratorServices(builder.Configuration);

builder.Services.AddApplication();

builder.Services.AddCorsPolicies();

builder.Services.AddAuthorizationPolicies();

builder.Services.AddDataProtectionConfiguration(builder.Configuration);

var app = builder.Build();

app.UseCorsPolicies();

app.MapDefaultEndpoints();

app.MapEndpointDefinitions();

//app.MapOpenApi();

app.UseSwagger();

app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "Attendance System API V1");
});

app.ApplyMigrations();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
