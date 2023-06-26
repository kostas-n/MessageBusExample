using MassTransit;
using MassTransit.Abstractions;
using MassTransit.RabbitMqTransport;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace MassTransitExample.RabbitMQ
{
	public static class RabbitMqExtensions
	{
		public static IServiceCollection ConfigureRMQBus(this IServiceCollection serviceCollection, BusConfig settings, Action<IServiceProvider, IBusFactoryConfigurator> addEndpoints)
		{
			var busProvider = new RabbitMqProvider(settings);
			serviceCollection.AddSingleton(serviceProvider =>
				busProvider.CreateBus(serviceProvider, addEndpoints)
			);

			return serviceCollection;
		}
	}
}