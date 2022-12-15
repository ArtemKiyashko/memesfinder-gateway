using System;
using Azure.Messaging.ServiceBus;

namespace MemesFinderGateway.Interfaces.AzureClients
{
	public interface IServiceBusClient
	{
        public ServiceBusSender CreateSender();
    }
}

