using MassTransit.Abstractions;
using MassTransit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MassTransitExample.ASB
{
	public class AsbProvider
	{
		private readonly BusConfig Config;

		public AsbProvider(BusConfig config)
		{
			Config = config;
		}
		public IBusControl CreateBus(IServiceProvider serviceProvider, Action<IServiceProvider, IBusFactoryConfigurator> addEndpoints)
		{
			return Bus.Factory.CreateUsingAzureServiceBus(cfg =>
			{
				cfg.Host(Config.AsUri());
				addEndpoints(serviceProvider, cfg);
			});
		}
	}
}
