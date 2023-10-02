using AsbExample.Persistence;
using Azure.Identity;
using Microsoft.Extensions.Options;

namespace AsbExample.Cosmos
{
	public interface ICosmosServiceFactory
	{
		ISamplesCosmosService GetCosmosService();
	}
	public class CosmosServiceFactory: ICosmosServiceFactory
	{
		private readonly CosmosConfig _config;

		public CosmosServiceFactory(IOptions<CosmosConfig> options)
		{
			_config = options.Value;
		}
		public ISamplesCosmosService GetCosmosService()
		{
			var databaseName = _config.DatabaseName;
			var containerName = _config.ContainerName;
			var account = _config.Account;
			var client = new Microsoft.Azure.Cosmos.CosmosClient(account, new ManagedIdentityCredential());
			var cosmosDbService = new SamplesCosmosService(client, databaseName, containerName);
			return cosmosDbService;
		}
	}
}
