using MassTransit;
using MassTransit.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace MassTransitExample.ASB
{
	public static class AsbExtensions
	{
		public static IServiceCollection ConfigureASB(this IServiceCollection serviceCollection, BusConfig settings, Action<IServiceProvider, IBusFactoryConfigurator> addEndpoints)
		{
			var busProvider = new AsbProvider(settings);
			serviceCollection.AddSingleton(serviceProvider =>
				busProvider.CreateBus(serviceProvider, addEndpoints)
			);

			return serviceCollection;
		}
	}
}