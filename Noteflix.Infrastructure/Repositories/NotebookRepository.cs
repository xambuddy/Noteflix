using Noteflix.Core.Entities;
using Noteflix.Core.Enums;
using Noteflix.Core.Repositories;
using Noteflix.Infrastructure.Context.Interfaces;

namespace Noteflix.Infrastructure.Repositories
{
    public class NotebookRepository : RepositoryBase<NotebookEntity>, INotebookRepository
    {
        public NotebookRepository(INotebooksContainerContext ctx) : base(ctx)
        {
        }

        public override EntityType Type => EntityType.Notebook;

        public override string GenerateId(NotebookEntity entity)
        {
            return $"{Guid.NewGuid()}";
        }
    }
}
