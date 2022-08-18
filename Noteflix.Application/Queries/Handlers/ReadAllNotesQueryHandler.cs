using MediatR;
using Noteflix.Core.Entities;
using Noteflix.Core.UnitOfWork;

namespace Noteflix.Application.Queries.Handlers
{
    public class ReadAllNotesQueryHandler : IRequestHandler<ReadAllNotesQuery, IEnumerable<NoteEntity>>
    {
        private readonly INotebookUnitOfWork notebookUnitOfWork;

        public ReadAllNotesQueryHandler(INotebookUnitOfWork notebookUnitOfWork)
        {
            this.notebookUnitOfWork = notebookUnitOfWork;
        }

        public async Task<IEnumerable<NoteEntity>> Handle(ReadAllNotesQuery request,
            CancellationToken cancellationToken)
        {
            var items = await notebookUnitOfWork.NoteRepository.GetAllItemsAsync();

            return items;
        }
    }
}
