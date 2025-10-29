using Aspire.Hosting;
using Microsoft.Extensions.Configuration;

var builder = DistributedApplication.CreateBuilder(args);

builder.AddDockerComposeEnvironment("env");

var sqlPassword = builder.AddParameter("SQLPassword", true);
var seqApiKey = builder.Configuration.GetValue<string>("Seq:ApiKey") ?? throw new InvalidOperationException("Seq API Key not configured");

var db = builder.AddSqlServer("SQLServer", sqlPassword, 1433)
    .WithDataVolume()
    .WithLifetime(ContainerLifetime.Persistent)
    .AddDatabase("AttendanceSystem");

var seq = builder.AddSeq("Seq", 5341)
    .ExcludeFromManifest()
    .WithDataVolume()
    .WithEnvironment("ACCEPT_EULA", "Y")
    .WithEnvironment("SEQ_FIRST_RUN_ADMIN_PASSWORD", seqApiKey)
    .WithLifetime(ContainerLifetime.Persistent);

var api = builder.AddProject<Projects.AttendanceSystem_Api>("AttendanceSystem-Api")
    .WithReference(db)
    .WaitFor(db)
    .WithReference(seq)
    .WaitFor(seq)
    .WithExternalHttpEndpoints();

builder.Build().Run();
