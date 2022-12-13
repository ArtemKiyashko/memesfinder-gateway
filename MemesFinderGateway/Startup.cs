using System;
using Azure.Identity;
using MemesFinderGateway.Extensions;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

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

            builder.Services.AddLogging();
        }
    }
}

