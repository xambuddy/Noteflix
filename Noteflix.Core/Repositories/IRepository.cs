using Ardalis.Specification;
using Noteflix.Core.Entities.Base;

namespace Noteflix.Core.Repositories
{
    public interface IRepository<T> where T : BaseEntity
    {
        Task<IEnumerable<T>> GetAllItemsAsync();

        Task<IEnumerable<T>> GetItemsAsync(string query);

        Task<IEnumerable<T>> GetItemsAsync(ISpecification<T> specification);

        Task<int> GetItemsCountAsync(ISpecification<T> specification);

        Task<T> GetItemAsync(string id, string partitionKey);

        T GetItem(string id);

        void AddItem(T item);

        void UpdateItem(T item);

        void DeleteItem(string id);
    }
}
