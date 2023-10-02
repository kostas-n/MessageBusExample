
using AsbExample;
using AsbExample.Cosmos;
using Microsoft.Extensions.DependencyInjection;

namespace AsbExample
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
			services.Configure<AsbConfig>(Configuration.GetSection("ASB"));
			services.Configure<CosmosConfig>(Configuration.GetSection("CosmosDb"));
			services.AddSingleton<IPublisher, Publisher>();
			services.AddHostedService<ListenerService>();
			services.AddSingleton<ICosmosServiceFactory, CosmosServiceFactory>();
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