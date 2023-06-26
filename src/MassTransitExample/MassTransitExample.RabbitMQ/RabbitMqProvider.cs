using MassTransit;
using MassTransit.Abstractions;
using MassTransit.RabbitMqTransport;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MassTransitExample.RabbitMQ
{
	public class RabbitMqProvider: IBusProvider
	{
		private readonly BusConfig Config;

		public RabbitMqProvider(BusConfig config)
		{
			Config = config;
		}
		public IBusControl CreateBus(IServiceProvider serviceProvider, Action<IServiceProvider, IBusFactoryConfigurator> addEndpoints)
		{
			return Bus.Factory.CreateUsingRabbitMq(cfg =>
			{
				cfg.Host(
					host: Config.Host,
					port: Config.Port,
					virtualHost: Config.VirtualHost, h =>
					{
						h.Username(Config.Username);
						h.Password(Config.Password);
					}
				);
				addEndpoints(serviceProvider, cfg);
			});
		}
	}
}
