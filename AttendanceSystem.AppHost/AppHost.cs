using Aspire.Hosting;
using Microsoft.Extensions.Configuration;

var builder = DistributedApplication.CreateBuilder(args);

builder.AddDockerComposeEnvironment("env");

var sqlPassword = builder.AddParameter("SQLPassword", true);
var rabbitmqUsername = builder.AddParameter("RabbitMQUsername", true);
var rabbitmqPassword = builder.AddParameter("RabbitMQPassword", true);
var seqApiKey = builder.Configuration.GetValue<string>("Seq:ApiKey") ?? throw new InvalidOperationException("Seq API Key not configured");

var db = builder.AddSqlServer("SQLServer", sqlPassword, 1433)
    .WithDataVolume()
    .WithLifetime(ContainerLifetime.Persistent)
    .AddDatabase("AttendanceSystem");

var seq = builder.AddSeq("Seq", 5341)
    .WithDataVolume()
    .WithEnvironment("ACCEPT_EULA", "Y")
    .WithEnvironment("SEQ_FIRST_RUN_ADMIN_PASSWORD", seqApiKey)
    .WithLifetime(ContainerLifetime.Persistent);

var rabbitmq = builder.AddRabbitMQ("RabbitMQ", rabbitmqUsername, rabbitmqPassword, 5672)
    .WithManagementPlugin()
    .WithDataVolume()
    .WithLifetime(ContainerLifetime.Persistent);

var api = builder.AddProject<Projects.AttendanceSystem_Api>("AttendanceSystem-Api")
    .WithReference(db)
    .WaitFor(db)
    .WithReference(seq)
    .WaitFor(seq)
    .WithReference(rabbitmq)
    .WaitFor(rabbitmq)
    .WithExternalHttpEndpoints();

var mailWorker = builder.AddProject<Projects.AttendanceSystem_Worker_Mail>("AttendanceSystem-Worker-Mail")
    .WithReference(rabbitmq)
    .WaitFor(rabbitmq);

builder.Build().Run();
