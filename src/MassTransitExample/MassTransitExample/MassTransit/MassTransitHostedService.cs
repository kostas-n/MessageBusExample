using MassTransit;

namespace MassTransitExample.MassTransit
{
	public class MassTransitHostedService : IHostedService
	{
		public MassTransitHostedService(IBusControl busControl, ILogger<MassTransitHostedService> logger)
		{
			BusControl = busControl;
			Logger = logger;
		}

		private IBusControl BusControl { get; }
		private ILogger<MassTransitHostedService> Logger { get; }

		public async Task StartAsync(CancellationToken cancellationToken)
		{
			await BusControl.StartAsync(cancellationToken);
		}

		public async Task StopAsync(CancellationToken cancellationToken)
		{
			await BusControl.StopAsync(cancellationToken);
		}
	}
}
