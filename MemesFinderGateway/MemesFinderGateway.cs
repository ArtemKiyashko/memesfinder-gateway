using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Telegram.Bot.Types;
using Azure.Messaging.ServiceBus;
using MemesFinderGateway.Extensions;
using MemesFinderGateway.Interfaces.AzureClients;
using MemesFinderGateway.Interfaces.DecisionMaker;
using System.Linq;

namespace MemesFinderGateway
{
    public class MemesFinderGateway
    {
        private readonly ILogger<MemesFinderGateway> _logger;
        private readonly IServiceBusClient _serviceBusClient;
        private readonly IDecisionMakerManager _deciscionMakerManager;

        public MemesFinderGateway(ILogger<MemesFinderGateway> logger, IServiceBusClient serviceBusClient, IDecisionMakerManager deciscionMakerManager)
        {
            _logger = logger;
            _serviceBusClient = serviceBusClient;
            _deciscionMakerManager = deciscionMakerManager;
        }

        [FunctionName("MemesFinderGateway")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] Update tgUpdate)
        {
            string messageString = tgUpdate.ToJson();
            _logger.LogInformation($"Update received: {messageString}");

            var decision = await _deciscionMakerManager.GetFinalDecisionAsync(tgUpdate);

            if (!decision.Decision)
                return new OkObjectResult(decision.Messages.Aggregate((f, s) => $"{f}{Environment.NewLine}{s}"));

            try
            {
                await using ServiceBusSender sender = _serviceBusClient.CreateSender();
                ServiceBusMessage serviceBusMessage = new(messageString);
                await sender.SendMessageAsync(serviceBusMessage);

                return new OkResult();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error sending to service bus: {0}", messageString);
                return new BadRequestObjectResult("Something went wrong, try again later");
            }
        }
    }
}

