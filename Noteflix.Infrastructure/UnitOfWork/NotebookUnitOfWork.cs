using Noteflix.Core.Entities.Base;
using Noteflix.Core.Repositories;
using Noteflix.Core.UnitOfWork;
using Noteflix.Infrastructure.Context.Interfaces;

namespace Noteflix.Infrastructure.UnitOfWork
{
    public class NotebookUnitOfWork : INotebookUnitOfWork
    {
        private readonly INotebooksContainerContext context;

        public NotebookUnitOfWork(
        INotebooksContainerContext notebooksContainerContext,
        INoteRepository noteRepository)
        {
            this.context = notebooksContainerContext;
            this.NoteRepository = noteRepository;
        }

        public INoteRepository NoteRepository { get; }

        public Task<List<BaseEntity>> CommitAsync(CancellationToken cancellationToken = default)
        {
            return this.context.SaveChangesAsync(cancellationToken);
        }
    }
}
