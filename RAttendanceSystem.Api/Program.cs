using RAttendanceSystem.Api.Endpoints;
using RAttendanceSystem.Application;
using RAttendanceSystem.Infrastructure;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApi();
builder.Services.AddInfrastructureServices();
builder.Services.AddApplicationServices();

builder.Services.AddAuthorization();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapEmployeeEndpoints();

app.Run();
