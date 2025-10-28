var builder = DistributedApplication.CreateBuilder(args);

var sqlPassword = builder.AddParameter("SQLPassword", true);

var sqlServer = builder.AddSqlServer("SQLServer", sqlPassword, 1433)
    .WithDataVolume()
    .WithLifetime(ContainerLifetime.Persistent);

var db = sqlServer.AddDatabase("AttendanceSystem");

builder.AddProject<Projects.AttendanceSystem_Api>("AttendanceSystem-Api")
    .WithReference(db)
    .WaitFor(db)
    .WithExternalHttpEndpoints();

builder.Build().Run();
