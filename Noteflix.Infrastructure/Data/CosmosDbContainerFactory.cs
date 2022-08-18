using Microsoft.Azure.Cosmos;
using Noteflix.Infrastructure.AppSettings;
using Noteflix.Infrastructure.Data.Interfaces;

namespace Noteflix.Infrastructure.Data
{
    public class CosmosDbContainerFactory : ICosmosDbContainerFactory
    {
        private readonly CosmosClient cosmosClient;
        private readonly string databaseName;
        private readonly List<ContainerInfo> containers;

        public CosmosDbContainerFactory(
            CosmosClient cosmosClient,
            string databaseName,
            List<ContainerInfo> containers)
        {
            this.databaseName = databaseName ?? throw new ArgumentNullException(nameof(databaseName));
            this.containers = containers ?? throw new ArgumentNullException(nameof(containers));
            this.cosmosClient = cosmosClient ?? throw new ArgumentNullException(nameof(cosmosClient));
        }

        public ICosmosDbContainer GetContainer(string containerName)
        {
            if (this.containers.Where(x => x.Name == containerName) == null)
            {
                throw new ArgumentException($"Unable to find container: {containerName}");
            }

            return new CosmosDbContainer(this.cosmosClient, this.databaseName, containerName);
        }

        public async Task EnsureDbSetupAsync()
        {
            var database = await this.cosmosClient.CreateDatabaseIfNotExistsAsync(this.databaseName);

            foreach (var container in this.containers)
            {
                await database.Database.CreateContainerIfNotExistsAsync(container.Name, $"{container.PartitionKey}");
            }
        }
    }
}
