using Microsoft.Azure.Cosmos;
using Noteflix.Core.Entities.Base;
using Noteflix.Core.Enums;
using Noteflix.Infrastructure.Context.Interfaces;
using Noteflix.Infrastructure.Data.Interfaces;
using System.Net;

namespace Noteflix.Infrastructure.Context
{
    public class NotebooksContainerContext : INotebooksContainerContext
    {
        private const string ContainerName = "notebooks";
        private readonly ICosmosDbContainerFactory cosmosDbContainerFactory;

        public NotebooksContainerContext(ICosmosDbContainerFactory cosmosDbContainerFactory)
        {
            this.cosmosDbContainerFactory = cosmosDbContainerFactory ?? throw new ArgumentNullException(nameof(cosmosDbContainerFactory));
            this.Container = this.cosmosDbContainerFactory.GetContainer(ContainerName).Container;
        }

        public Container Container { get; }

        public List<BaseEntity> DataObjects { get; } = new();

        public void Reset() => this.DataObjects.Clear();

        public void Add(BaseEntity entity)
        {
            if (this.DataObjects.FindIndex(
                0,
                o => o.Id == entity.Id && o.PartitionKey == entity.PartitionKey) == -1)
            {
                this.DataObjects.Add(entity);
            }
        }

        public async Task<List<BaseEntity>> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            switch (this.DataObjects.Count)
            {
                case 1:
                    {
                        var result = await this.SaveSingleAsync(this.DataObjects[0], cancellationToken);
                        return result;
                    }

                case > 1:
                    {
                        var result = await this.SaveInTransactionalBatchAsync(cancellationToken);
                        return result;
                    }

                default:
                    return new List<BaseEntity>();
            }
        }

        private async Task<List<BaseEntity>> SaveInTransactionalBatchAsync(
            CancellationToken cancellationToken)
        {
            if (this.DataObjects.Count > 0)
            {
                var pk = new PartitionKey(this.DataObjects[0].PartitionKey);
                var tb = this.Container.CreateTransactionalBatch(pk);
                this.DataObjects.ForEach(o =>
                {
                    TransactionalBatchItemRequestOptions tro = null;

                    switch (o.State)
                    {
                        case EntityState.Created:
                            tb.CreateItem(o);
                            break;
                        case EntityState.Updated or EntityState.Deleted:
                            tb.ReplaceItem(o.Id, o, tro);
                            break;
                        default:
                            break;
                    }
                });

                var tbResult = await tb.ExecuteAsync(cancellationToken);

                if (!tbResult.IsSuccessStatusCode)
                {
                    for (var i = 0; i < this.DataObjects.Count; i++)
                    {
                        if (tbResult[i].StatusCode != HttpStatusCode.FailedDependency)
                        {
                            // Not recoverable - clear context
                            this.DataObjects.Clear();
                        }
                    }
                }
            }

            var result = new List<BaseEntity>(this.DataObjects); // return copy of list as result

            // work has been successfully done - reset DataObjects list
            this.DataObjects.Clear();
            return result;
        }

        private async Task<List<BaseEntity>> SaveSingleAsync(
            BaseEntity dObj,
            CancellationToken cancellationToken = default)
        {
            var reqOptions = new ItemRequestOptions
            {
                EnableContentResponseOnWrite = false,
            };

            var pk = new PartitionKey(dObj.PartitionKey);

            try
            {
                ItemResponse<BaseEntity> response;

                switch (dObj.State)
                {
                    case EntityState.Created:
                        response = await this.Container.CreateItemAsync(dObj, pk, reqOptions, cancellationToken);
                        break;
                    case EntityState.Updated:
                    case EntityState.Deleted:
                        response = await this.Container.ReplaceItemAsync(dObj, dObj.Id, pk, reqOptions, cancellationToken);
                        break;
                    case EntityState.Unmodified:
                        break;
                    default:
                        this.DataObjects.Clear();
                        return new List<BaseEntity>();
                }

                var result = new List<BaseEntity>(1) { dObj };

                // work has been successfully done - reset DataObjects list
                this.DataObjects.Clear();
                return result;
            }
            catch (CosmosException e)
            {
                // Not recoverable - clear context
                this.DataObjects.Clear();
            }

            return null;
        }
    }
}