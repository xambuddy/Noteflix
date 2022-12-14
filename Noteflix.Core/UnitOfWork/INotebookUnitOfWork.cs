using Noteflix.Core.Entities.Base;
using Noteflix.Core.Repositories;

namespace Noteflix.Core.UnitOfWork
{
    public interface INotebookUnitOfWork
    {
        INoteRepository NoteRepository { get; }

        INotebookRepository NotebookRepository { get; }

        Task<List<BaseEntity>> CommitAsync(CancellationToken cancellationToken = default);
    }
}
