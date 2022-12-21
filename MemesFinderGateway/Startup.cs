using Azure.Identity;
using MemesFinderGateway.AzureClients;
using MemesFinderGateway.Extensions;
using MemesFinderGateway.Infrastructure.DependencyInjection;
using MemesFinderGateway.Interfaces.AzureClients;
using MemesFinderGateway.Options;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Azure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

[assembly: FunctionsStartup(typeof(MemesFinderGateway.Startup))]
namespace MemesFinderGateway
{
	public class Startup : FunctionsStartup
    {
        private IConfigurationRoot _functionConfig;

        public override void Configure(IFunctionsHostBuilder builder)
        {
            _functionConfig = new ConfigurationBuilder()
                .AddEnvironmentVariables()
                .Build();

            builder.Services.Configure<ServiceBusOptions>(_functionConfig.GetSection("ServiceBusOptions"));

            builder.Services.AddAzureClients(clientBuilder =>
            {
                var provider = builder.Services.BuildServiceProvider();

                clientBuilder.UseCredential(new DefaultAzureCredential());
                clientBuilder.AddServiceBusClientWithNamespace(provider.GetRequiredService<IOptions<ServiceBusOptions>>().Value.FullyQualifiedNamespace);
            });

            builder.Services.AddTransient<IServiceBusClient, ServiceBusAllMessagesClient>();

            builder.Services.AddDecisionManager(_functionConfig);

            builder.Services
                .AddLogging()
                .AddScoped<ILogger>(provider => provider.GetRequiredService<ILogger<MemesFinderGateway>>());
        }
    }
}

