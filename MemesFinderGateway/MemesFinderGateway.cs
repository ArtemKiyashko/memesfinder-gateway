using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Telegram.Bot.Types;
using Azure.Messaging.ServiceBus;
using Microsoft.Extensions.Options;
using MemesFinderGateway.Options;
using MemesFinderGateway.Extensions;

namespace MemesFinderGateway
{
    public class MemesFinderGateway
    {
        private readonly ServiceBusClient _serviceBusClient;
        private readonly ServiceBusOptions _options;

        public MemesFinderGateway(ServiceBusClient serviceBusClient, IOptions<ServiceBusOptions> options)
        {
            _serviceBusClient = serviceBusClient;
            _options = options.Value;
        }

        [FunctionName("MemesFinderGateway")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] Update tgUpdate,
            ILogger log)
        {
            string messageString = tgUpdate.ToJson();
            log.LogInformation($"Update received: {messageString}");

            try
            {
                await using ServiceBusSender sender = _serviceBusClient.CreateSender(_options.TargetTopicName);
                ServiceBusMessage serviceBusMessage = new ServiceBusMessage(tgUpdate.ToJson());
                await sender.SendMessageAsync(serviceBusMessage);

                return new OkResult();
            }
            catch (Exception ex)
            {
                log.LogError(ex, "Error sending to service bus: {0}", messageString);
                return new BadRequestObjectResult("Something went wrong, try again later");
            }
        }
    }
}

