using System;
using Azure.Identity;
using MemesFinderGateway.Extensions;
using MemesFinderGateway.Options;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Azure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

[assembly: FunctionsStartup(typeof(MemesFinderGateway.Startup))]
namespace MemesFinderGateway
{
	public class Startup : FunctionsStartup
    {
        private IConfigurationRoot _functionConfig;

        public override void Configure(IFunctionsHostBuilder builder)
        {
            var configBuilder = new ConfigurationBuilder()
                .AddEnvironmentVariables();

            if (builder.IsProduction())
                configBuilder.AddAzureKeyVault(
                    new Uri($"https://{builder.GetContext().Configuration["KeyVaultName"]}.vault.azure.net/"),
                    new DefaultAzureCredential());

            _functionConfig = configBuilder.Build();

            builder.Services.Configure<ServiceBusOptions>(_functionConfig.GetSection("ServiceBusOptions"));

            builder.Services.AddAzureClients(clientBuilder =>
            {
                var provider = builder.Services.BuildServiceProvider();

                clientBuilder.UseCredential(new DefaultAzureCredential());
                clientBuilder.AddServiceBusClientWithNamespace(provider.GetRequiredService<IOptions<ServiceBusOptions>>().Value.FullyQualifiedNamespace);
            });

            builder.Services.AddLogging();
        }
    }
}

