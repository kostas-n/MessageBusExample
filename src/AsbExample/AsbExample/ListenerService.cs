using AsbExample.Cosmos;
using AsbExample.Persistence;
using Azure.Core;
using Azure.Identity;
using Azure.Messaging.ServiceBus;
using Microsoft.Azure.Amqp.Framing;
using Microsoft.Extensions.Options;
using System.Diagnostics;
using System.Text.Json;

namespace AsbExample
{
	public class ListenerService : IHostedService
	{
		private ServiceBusProcessor _processor;
		private ICosmosServiceFactory _cosmosServiceFactory;
		private FailureRandomiser _failureRandomiser;

		public ListenerService(ICosmosServiceFactory cosmosServiceFactory, IOptions<AsbConfig> config)
		{
			var client = new ServiceBusClient(config.Value.Namespace, new DefaultAzureCredential());

			_processor = client.CreateProcessor(config.Value.Queue);
			_cosmosServiceFactory = cosmosServiceFactory;
			_failureRandomiser = new FailureRandomiser();
		}

		public async Task StartAsync(CancellationToken cancellationToken)
		{
			await _processor.StartProcessingAsync();
			_processor.ProcessMessageAsync += _processor_ProcessMessageAsync;
			_processor.ProcessErrorAsync += _processor_ProcessErrorAsync;
		}

		private Task _processor_ProcessErrorAsync(ProcessErrorEventArgs arg)
		{
			return Task.CompletedTask;
		}

		private async Task _processor_ProcessMessageAsync(ProcessMessageEventArgs arg)
		{
			if (_failureRandomiser.ShoudlFail())
			{
				await arg.DeadLetterMessageAsync(arg.Message);
				return;
			}
			var sample = JsonSerializer.Deserialize<SampleDto>(arg.Message.Body.ToString());

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
