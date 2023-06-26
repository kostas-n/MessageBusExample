using MassTransit;
using MassTransit.Abstractions;
using MassTransitExample.ASB;
using MassTransitExample.MassTransit;
using MassTransitExample.Messages;
using MassTransitExample.RabbitMQ;
using Microsoft.Extensions.DependencyInjection;

namespace MassTransitExample
{
	public class Startup
	{
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		/// <summary>
		/// Configures the services in the container.
		/// </summary>
		/// <param name="services"></param>
		public void ConfigureServices(IServiceCollection services)
		{
			services.AddControllers();

			var busConfig = Configuration.GetSection("RabbitMQ").Get<BusConfig>();
			services.AddMassTransit(cfg =>
			{
				cfg.AddConsumer<SampleAddedListener>();
			});

			//services.ConfigureRMQBus(busConfig, addEndpoints: (provider, cfg) =>
			//{
			//	cfg.ReceiveEndpoint($"{typeof(ISampleAdded).FullName}", endpoint =>
			//	{
			//		endpoint.Consumer<SampleAddedListener>(provider);
			//	});
			//});
			services.ConfigureASB(busConfig, addEndpoints: (provider, cfg) =>
			{
				cfg.ReceiveEndpoint($"{typeof(ISampleAdded).FullName}", endpoint =>
				{
					endpoint.Consumer<SampleAddedListener>(provider);
				});
			});


			services.AddSingleton<ISamplePublisher, SamplePublisher>();
			services.AddHostedService<MassTransitHostedService>();
		}

		/// <summary>
		/// Configures the HTTP request pipeline
		/// </summary>
		/// <param name="app"></param>
		/// <param name="env"></param>
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (!env.IsDevelopment())
			{
				app.UseHttpsRedirection();
			}
			app.UseRouting();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllers();
			});
		}
	}
}
