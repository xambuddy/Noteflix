using Microsoft.Azure.Cosmos;
using Noteflix.Core.Entities.Base;

namespace Noteflix.Infrastructure.Context.Interfaces
{
    public interface INotebooksContainerContext
    {
        public Container Container { get; }

        public List<BaseEntity> DataObjects { get; }

        public void Add(BaseEntity entity);

        public Task<List<BaseEntity>> SaveChangesAsync(CancellationToken cancellationToken = default);

        public void Reset();
    }
}
