using Azure.Identity;
using Azure.Monitor.OpenTelemetry.Exporter;
using MemesFinderGateway.AzureClients;
using MemesFinderGateway.Infrastructure.DependencyInjection;
using MemesFinderGateway.Interfaces.AzureClients;
using MemesFinderGateway.Options;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Builder;
using Microsoft.Azure.Functions.Worker.OpenTelemetry;
using Microsoft.Extensions.Azure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Azure.Core.Serialization;
using OpenTelemetry.Trace;

var builder = FunctionsApplication.CreateBuilder(args);

AppContext.SetSwitch("Azure.Experimental.EnableActivitySource", true);

builder.Services.AddOpenTelemetry()
    .WithTracing(tracing => tracing.AddSource("Azure.Messaging.ServiceBus.*"))
    .UseFunctionsWorkerDefaults()
    .UseAzureMonitorExporter();

builder.Services.Configure<WorkerOptions>(options =>
    options.Serializer = new NewtonsoftJsonObjectSerializer());

builder.Services.Configure<ServiceBusOptions>(builder.Configuration.GetSection("ServiceBusOptions"));
builder.Services.AddAzureClients(clientBuilder =>
{
    clientBuilder.UseCredential(new DefaultAzureCredential());
    clientBuilder.AddServiceBusClientWithNamespace(
        builder.Configuration["ServiceBusOptions:FullyQualifiedNamespace"]!);
});
builder.Services.AddTransient<IServiceBusClient, ServiceBusAllMessagesClient>();
builder.Services.AddDecisionManager(builder.Configuration);

builder.Build().Run();
