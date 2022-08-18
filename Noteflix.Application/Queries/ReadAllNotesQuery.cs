using MediatR;
using Noteflix.Core.Entities;

namespace Noteflix.Application.Queries
{
    public class ReadAllNotesQuery : IRequest<IEnumerable<NoteEntity>>
    {
    }
}
