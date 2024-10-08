using Consul;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ServiceA;

var builder = WebApplication.CreateBuilder(args);

// Add Consul client to the service container
builder.Services.AddSingleton<IConsulClient, ConsulClient>(p => new ConsulClient(consulConfig =>
{
    var address = builder.Configuration["ConsulConfig:Address"] ?? "http://localhost:8500";
    consulConfig.Address = new Uri(address);
}));

// Add the ConsulHostedService to register the service with Consul
builder.Services.AddHostedService<ConsulHostedService>();

// Access configuration to get the dynamic port for the service
var servicePort = int.Parse(builder.Configuration["ConsulConfig:ServicePort"] ?? "5002");

// Configure Kestrel to listen on the specified port
builder.WebHost.ConfigureKestrel(serverOptions =>
{
    serverOptions.ListenAnyIP(servicePort);  // Bind the service to the dynamic port
});

// Logging the port for easier debugging
Console.WriteLine($"Service is running on port {servicePort}");

var app = builder.Build();

// Add health check and base route
app.MapGet("/", () => "PrductService is running...");
app.MapGet("/api/health", () => Results.Ok("Service is healthy"));

app.Run();