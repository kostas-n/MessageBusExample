using MassTransit;
using MassTransit.Registration;

namespace MassTransit.Abstractions
{
	public interface IBusProvider
	{
		public IBusControl CreateBus(IServiceProvider serviceProvider, Action<IServiceProvider, IBusFactoryConfigurator> addEndpoints);
	}
}