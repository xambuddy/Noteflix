using MediatR;
using Noteflix.Application.Responses;
using Noteflix.Core.Entities;
using Noteflix.Core.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Noteflix.Application.Commands.Handlers
{
    public class CreateNotebookCommandHandler : IRequestHandler<CreateNotebookCommand, CreateNotebookCommandResponse>
    {
        private readonly INotebookUnitOfWork notebookUnitOfWork;
        public CreateNotebookCommandHandler(INotebookUnitOfWork notebookUnitOfWork)
        {
            this.notebookUnitOfWork = notebookUnitOfWork;
        }
        public async Task<CreateNotebookCommandResponse> Handle(CreateNotebookCommand request, CancellationToken cancellationToken)
        {
            var notebook = new NotebookEntity
            {
                Title = request.Title
            };

            this.notebookUnitOfWork.NotebookRepository.AddItem(notebook);

            var result = await this.notebookUnitOfWork.CommitAsync(cancellationToken);

            var cResult = result.FirstOrDefault(r => r is NotebookEntity);
            if (cResult != null)
            {
                return new CreateNotebookCommandResponse(cResult.Id);
            }

            throw new Exception("Error saving notebook");
        }
    }
}
