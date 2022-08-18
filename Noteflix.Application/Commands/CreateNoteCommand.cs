using MediatR;
using Noteflix.Application.Responses;
using Noteflix.Core.Entities;
using Noteflix.Core.Entities.SubEntities;

namespace Noteflix.Application.Commands
{
    public class CreateNoteCommand : IRequest<CreateNoteCommandResponse>
    {
        public string Title { get; set; }

        public string Content { get; set; }

        public User CreatedBy { get; set; }

        public IEnumerable<NoteTask> Tasks { get; set; }
    }
}
