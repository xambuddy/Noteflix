using MediatR;
using Noteflix.Application.Responses;
using Noteflix.Core.Entities;
using Noteflix.Core.UnitOfWork;

namespace Noteflix.Application.Commands.Handlers
{
    public class CreateNoteCommandHandler : IRequestHandler<CreateNoteCommand, CreateNoteCommandResponse>
    {
        private readonly INotebookUnitOfWork notebookUnitOfWork;
        public CreateNoteCommandHandler(INotebookUnitOfWork notebookUnitOfWork)
        {
            this.notebookUnitOfWork = notebookUnitOfWork;
        }
        public async Task<CreateNoteCommandResponse> Handle(CreateNoteCommand request, CancellationToken cancellationToken)
        {
            var note = new NoteEntity
            {
                Title = request.Title,
                Content = request.Content,
                CreatedBy = request.CreatedBy,
                Tasks = request.Tasks,
            };

            this.notebookUnitOfWork.NoteRepository.AddItem(note);

            var result = await this.notebookUnitOfWork.CommitAsync(cancellationToken);

            var cResult = result.FirstOrDefault(r => r is NoteEntity);
            if (cResult != null)
            {
                return new CreateNoteCommandResponse(cResult.Id);
            }

            throw new Exception("Error saving note");
        }
    }
}
