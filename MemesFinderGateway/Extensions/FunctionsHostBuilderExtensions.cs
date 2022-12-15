using System;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace MemesFinderGateway.Extensions
{
    public static class FunctionsHostBuilderExtensions
    {
        public static bool IsProduction(this IFunctionsHostBuilder builder)
            => builder.GetContext().EnvironmentName.Equals("Production", StringComparison.OrdinalIgnoreCase);

        public static IFunctionsHostBuilder UseApplicationInsights(this IFunctionsHostBuilder builder, IConfiguration configuration)
        {
            if (!builder.IsProduction())
                return builder;

            builder.Services.AddLogging(logBuilder =>
            {
                logBuilder.AddApplicationInsights(
                    configureTelemetryConfiguration: (config) => config.ConnectionString = configuration.GetValue<string>("APPLICATIONINSIGHTS_CONNECTION_STRING"),
                    configureApplicationInsightsLoggerOptions: (options) => { });
            });
            return builder;
        }
    }
}

