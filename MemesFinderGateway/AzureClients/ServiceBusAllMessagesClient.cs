using System;
using Azure.Messaging.ServiceBus;
using MemesFinderGateway.Interfaces.AzureClients;
using MemesFinderGateway.Options;
using Microsoft.Extensions.Options;

namespace MemesFinderGateway.AzureClients
{
	public class ServiceBusAllMessagesClient : IServiceBusClient
    {
        private readonly ServiceBusClient _serviceBusClient;
        private readonly ServiceBusOptions _serviceBusOptions;

        public ServiceBusAllMessagesClient(ServiceBusClient serviceBusClient, IOptions<ServiceBusOptions> options)
		{
            _serviceBusClient = serviceBusClient;
            _serviceBusOptions = options.Value;
        }

        public ServiceBusSender CreateSender()
            => _serviceBusClient.CreateSender(_serviceBusOptions.AllMessagesTopic);
    }
}

