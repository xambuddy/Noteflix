using Ardalis.Specification;
using Ardalis.Specification.EntityFrameworkCore;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Linq;
using Noteflix.Core.Entities.Base;
using Noteflix.Core.Enums;
using Noteflix.Core.Repositories;
using Noteflix.Infrastructure.Context.Interfaces;
using System.Net;

namespace Noteflix.Infrastructure.Repositories
{
    public abstract class RepositoryBase<T> : IRepository<T>
    where T : BaseEntity
    {
        private readonly ISpecificationEvaluator specificationEvaluator;
        public RepositoryBase(INotebooksContainerContext notebooksContainerContext)
        {
            this.NotebooksContext = notebooksContainerContext;
            this.specificationEvaluator = SpecificationEvaluator.Default;
        }

        public INotebooksContainerContext NotebooksContext { get; }

        public abstract string GenerateId(T entity);

        public abstract EntityType Type { get; }

        public void AddItem(T item)
        {
            item.Id = this.GenerateId(item);
            item.Type = Type;
            item.Ttl = -1;
            item.State = EntityState.Created;
            item.SetPartitionKey();

            this.NotebooksContext.Add(item);
        }

        public void DeleteItem(string id)
        {
            try
            {
                var result = GetItem(id);

                result.SetDeleted();
                result.State = EntityState.Deleted;

                this.NotebooksContext.Add(result);
            }
            catch (CosmosException e)
            {

            }
        }

        public async Task<T> GetItemAsync(string id, string partitionKey)
        {
            try
            {
                var response = await this.NotebooksContext.Container.ReadItemAsync<T>(id, this.ResolvePartitionKey(partitionKey));
                return response.Resource;
            }
            catch (CosmosException ex) when (ex.StatusCode == HttpStatusCode.NotFound)
            {
                return null;
            }
        }

        public T GetItem(string id)
        {
            try
            {
                var queryable = this.NotebooksContext.Container.GetItemLinqQueryable<T>(true);
                return queryable.Where(x => x.Id == id)?.AsEnumerable()?.FirstOrDefault();
            }
            catch (CosmosException ex) when (ex.StatusCode == HttpStatusCode.NotFound)
            {
                return null;
            }
        }

        public async Task<IEnumerable<T>> GetAllItemsAsync()
        {
            var resultSetIterator = this.NotebooksContext.Container.GetItemQueryIterator<T>();
            var results = new List<T>();
            while (resultSetIterator.HasMoreResults)
            {
                var response = await resultSetIterator.ReadNextAsync();

                results.AddRange(response.ToList());
            }

            return results;
        }

        public async Task<IEnumerable<T>> GetItemsAsync(string query)
        {
            var resultSetIterator = this.NotebooksContext.Container.GetItemQueryIterator<T>(new QueryDefinition(query));
            var results = new List<T>();
            while (resultSetIterator.HasMoreResults)
            {
                var response = await resultSetIterator.ReadNextAsync();

                results.AddRange(response.ToList());
            }

            return results;
        }

        public async Task<IEnumerable<T>> GetItemsAsync(ISpecification<T> specification)
        {
            var queryable = this.ApplySpecification(specification);
            var iterator = queryable.ToFeedIterator();

            var results = new List<T>();
            while (iterator.HasMoreResults)
            {
                var response = await iterator.ReadNextAsync();

                results.AddRange(response.ToList());
            }

            return results;
        }

        public async Task<int> GetItemsCountAsync(ISpecification<T> specification)
        {
            var queryable = this.ApplySpecification(specification);
            return await queryable.CountAsync();
        }

        public void UpdateItem(T item)
        {
            item.State = EntityState.Updated;

            this.NotebooksContext.Add(item);
        }

        private IQueryable<T> ApplySpecification(ISpecification<T> specification) => 
            this.specificationEvaluator.GetQuery(this.NotebooksContext.Container.GetItemLinqQueryable<T>(), specification);

        private PartitionKey ResolvePartitionKey(string entityId) => new(entityId);
    }
}
