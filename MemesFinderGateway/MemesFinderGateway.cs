using Azure.Messaging.ServiceBus;
using MemesFinderGateway.Extensions;
using MemesFinderGateway.Interfaces.AzureClients;
using MemesFinderGateway.Interfaces.DecisionMaker;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace MemesFinderGateway
{
    public class MemesFinderGateway
    {
        private readonly IServiceBusClient _serviceBusClient;
        private readonly IDecisionMakerManager _deciscionMakerManager;
        private readonly ILogger<MemesFinderGateway> _logger;

        public MemesFinderGateway(IServiceBusClient serviceBusClient, IDecisionMakerManager deciscionMakerManager, ILogger<MemesFinderGateway> logger)
        {
            _serviceBusClient = serviceBusClient;
            _deciscionMakerManager = deciscionMakerManager;
            _logger = logger;
        }

        [Function("MemesFinderGateway")]
        public async Task<HttpResponseData> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequestData request,
            [FromBody] Update tgUpdate)
        {
            if (tgUpdate is null)
            {
                var invalidResponse = request.CreateResponse(HttpStatusCode.BadRequest);
                await invalidResponse.WriteStringAsync("Invalid Telegram update");
                return invalidResponse;
            }

            string messageString = tgUpdate.ToJson();
            _logger.LogInformation("Update received: {TelegramUpdate}", messageString);

            var decision = await _deciscionMakerManager.GetFinalDecisionAsync(tgUpdate);

            if (!decision.Decision)
                return await HandleNegativeDecision(request, decision);

            return await SendMessageToServiceBus(request, messageString);
        }

        private async Task<HttpResponseData> SendMessageToServiceBus(HttpRequestData request, string messageString)
        {
            try
            {
                await using ServiceBusSender sender = _serviceBusClient.CreateSender();
                ServiceBusMessage serviceBusMessage = new(messageString);
                await sender.SendMessageAsync(serviceBusMessage);

                return request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error sending Telegram update to Service Bus");
                var response = request.CreateResponse(HttpStatusCode.BadRequest);
                await response.WriteAsJsonAsync("Something went wrong, try again later");
                return response;
            }
        }

        private async Task<HttpResponseData> HandleNegativeDecision(HttpRequestData request, DecisionManagerResult decision)
        {
            var aggregatedMessages = decision.Messages
                .Aggregate((f, s) => $"{f}{Environment.NewLine}{s}");
            _logger.LogInformation("Negative decision taken: {DecisionMessages}", aggregatedMessages);
            var response = request.CreateResponse(HttpStatusCode.OK);
            await response.WriteAsJsonAsync(aggregatedMessages);
            return response;
        }
    }
}

