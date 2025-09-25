using Microsoft.IdentityModel.Tokens;
using RAttendanceSystem.Api.Endpoints;
using RAttendanceSystem.Application;
using RAttendanceSystem.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddInfrastructureServices();
builder.Services.AddApplicationServices();
builder.Services.AddAuthentication()
    .AddJwtBearer(options =>
    {
        var jwksUri = $"{builder.Configuration["Keycloak:Domain"]}/realms/{builder.Configuration["Keycloak:Realm"]}/protocol/openid-connect/certs";
        options.TokenValidationParameters = new TokenValidationParameters
        {
            RoleClaimType = "realm_access.roles",
            NameClaimType = "preferred_username",
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuers = builder.Configuration.GetSection("Authentication:Authorities").Get<string[]>(),
            ValidAudiences = builder.Configuration.GetSection("Authentication:Audiences").Get<string[]>(),
            IssuerSigningKeyResolver = (token, securityToken, kid, parameters) =>
            {
                using var client = new HttpClient();
                var response = client.GetStringAsync(jwksUri).Result;
                var keys = new JsonWebKeySet(response);
                return keys.Keys.Where(k => k.Kid == kid);
            }
        };
        options.RequireHttpsMetadata = false;
    });
builder.Services.AddAuthorization();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapEmployeeEndpoints();
app.MapDepartmentEndpoints();
app.MapTitleEndpoints();
app.MapSystemNotificationEndpoints();
app.MapAttendanceWiFiEndpoints();
app.MapAttendanceEndpoints();

app.Run();