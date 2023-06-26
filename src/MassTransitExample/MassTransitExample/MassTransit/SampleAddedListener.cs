using MassTransit;
using MassTransitExample.Messages;

namespace MassTransitExample.MassTransit
{
	public class SampleAddedListener : IConsumer<ISampleAdded>
	{
		private readonly ILogger<SampleAddedListener> Logger;
		public SampleAddedListener(ILogger<SampleAddedListener> logger)
		{
			Logger = logger;
		}
		public Task Consume(ConsumeContext<ISampleAdded> context)
		{
			Logger.LogInformation($"consuming a message for sample with id {context.Message.SampleId} consumer");
			return Task.CompletedTask;
		}
	}
}
