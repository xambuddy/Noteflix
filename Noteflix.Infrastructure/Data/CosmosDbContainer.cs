using Microsoft.Azure.Cosmos;
using Noteflix.Infrastructure.Data.Interfaces;

namespace Noteflix.Infrastructure.Data
{
    public class CosmosDbContainer : ICosmosDbContainer
    {
        public CosmosDbContainer(
            CosmosClient cosmosClient,
            string databaseName,
            string containerName) => this.Container = cosmosClient.GetContainer(databaseName, containerName);

        public Container Container { get; }
    }
}
