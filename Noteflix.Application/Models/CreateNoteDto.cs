using Noteflix.Core.Entities;
using Noteflix.Core.Entities.SubEntities;

namespace Noteflix.Application.Models
{
    public class CreateNoteDto
    {
        public string Title { get; set; }

        public string Content { get; set; }

        public User CreatedBy { get; set; }

        public IEnumerable<NoteTask> Tasks { get; set; }
    }
}
