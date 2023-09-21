using AsbExample.Messages;
using Azure.Identity;
using Azure.Messaging.ServiceBus;
using Microsoft.Extensions.Options;

namespace AsbExample
{
	public interface IPublisher
	{
		Task Publish(ISampleAdded sampleAddedMessage);
	}
	public class Publisher : IPublisher
	{
		private readonly ServiceBusClient _client;
		private readonly ServiceBusSender _sender;

		public Publisher()
		{
			_client = new ServiceBusClient("namespace", new DefaultAzureCredential());
			_sender = _client.CreateSender("queue");
		}

		public async Task Publish(ISampleAdded sampleAddedMessage)
		{
			using ServiceBusMessageBatch messageBatch = await _sender.CreateMessageBatchAsync();

			var msg = new ServiceBusMessage(BinaryData.FromObjectAsJson(sampleAddedMessage));
			messageBatch.TryAddMessage(msg);

			try
			{
				await _sender.SendMessagesAsync(messageBatch);
			}
			finally
			{
				// Calling DisposeAsync on client types is required to ensure that network
				// resources and other unmanaged objects are properly cleaned up.
				await _sender.DisposeAsync();
				await _client.DisposeAsync();
			}
		}
	}
}
