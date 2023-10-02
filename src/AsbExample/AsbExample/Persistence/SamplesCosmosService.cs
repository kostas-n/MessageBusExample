using Microsoft.Azure.Cosmos;

namespace AsbExample.Persistence
{
	public interface ISamplesCosmosService
	{
		Task AddSample(SampleDto sample);
	}

	public class SamplesCosmosService: ISamplesCosmosService
	{
		private Container _container;

		public SamplesCosmosService(CosmosClient cosmosClient, string databaseName, string containerName)
		{
			_container = cosmosClient.GetContainer(databaseName, containerName);
		}

		public async Task AddSample(SampleDto sample)
		{
			await _container.CreateItemAsync(sample, new PartitionKey(sample.SampleId));
		}
	}
}
