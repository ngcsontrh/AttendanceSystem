var builder = DistributedApplication.CreateBuilder(args);

builder.AddDockerComposeEnvironment("env");

var sqlPassword = builder.AddParameter("SQLPassword", true);

var db = builder.AddSqlServer("SQLServer", sqlPassword, 1433)
    .WithDataVolume()
    .WithLifetime(ContainerLifetime.Persistent)
    .AddDatabase("AttendanceSystem");

builder.AddProject<Projects.AttendanceSystem_Api>("AttendanceSystem-Api")
    .WithReference(db)
    .WaitFor(db)
    .WithExternalHttpEndpoints();

builder.Build().Run();
