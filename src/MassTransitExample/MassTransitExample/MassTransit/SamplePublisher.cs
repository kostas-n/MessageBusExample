using MassTransit;
using MassTransit.Testing;
using MassTransitExample.Messages;

namespace MassTransitExample.MassTransit
{
	public interface ISamplePublisher
	{
		Task Publish(ISampleAdded sampleAddedMessage);
	}
	public class SamplePublisher : ISamplePublisher
	{
		private ILogger<SamplePublisher> Logger { get; }
		private readonly IBusControl BusControl;

		public SamplePublisher(IBusControl busControl, ILogger<SamplePublisher> logger)
		{
			BusControl = busControl;
			Logger = logger;
		}
		public async Task Publish(ISampleAdded sampleAddedMessage)
		{
			Logger.LogInformation($"publishing a message for sample with id {sampleAddedMessage.SampleId}");
			await BusControl.Publish(sampleAddedMessage);
		}
	}
}
