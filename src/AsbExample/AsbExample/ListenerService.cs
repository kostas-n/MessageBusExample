using AsbExample.Cosmos;
using AsbExample.Persistence;
using Azure.Core;
using Azure.Identity;
using Azure.Messaging.ServiceBus;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace AsbExample
{
	public class ListenerService: IHostedService
	{
		private ServiceBusProcessor _processor;
		private ICosmosServiceFactory _cosmosServiceFactory;

		public ListenerService(ICosmosServiceFactory cosmosServiceFactory, IOptions<AsbConfig> config)
		{
			var clientOptions = new ServiceBusClientOptions
			{
				TransportType = ServiceBusTransportType.AmqpWebSockets
			};
			var client = new ServiceBusClient(config.Value.Namespace, new DefaultAzureCredential(), clientOptions);

			_processor = client.CreateProcessor(config.Value.Queue);
			_cosmosServiceFactory = cosmosServiceFactory;
		}

		public async Task StartAsync(CancellationToken cancellationToken)
		{
			await _processor.StartProcessingAsync();
			_processor.ProcessMessageAsync += _processor_ProcessMessageAsync;
		}

		private async Task _processor_ProcessMessageAsync(ProcessMessageEventArgs arg)
		{
			var sample =  JsonSerializer.Deserialize<SampleDto>(arg.Message.Body.ToString());

			var cosmosService = _cosmosServiceFactory.GetCosmosService();
			await cosmosService.AddSample(sample);

			await arg.CompleteMessageAsync(arg.Message);
		}

		public async Task StopAsync(CancellationToken cancellationToken)
		{
			if (_processor != null)
			{
				await _processor.CloseAsync();
				await _processor.DisposeAsync();
			}
		}
	}
}
