using Microsoft.Azure.Cosmos;
using Noteflix.Core.Entities;
using Noteflix.Core.Enums;
using Noteflix.Core.Repositories;
using Noteflix.Infrastructure.Context.Interfaces;

namespace Noteflix.Infrastructure.Repositories
{
    public class NoteRepository : RepositoryBase<NoteEntity>, INoteRepository
    {
        public NoteRepository(INotebooksContainerContext ctx) : base(ctx)
        {
        }

        public override EntityType Type => EntityType.Note;

        public override string GenerateId(NoteEntity entity)
        {
            return $"{Guid.NewGuid()}";
        }
    }
}
