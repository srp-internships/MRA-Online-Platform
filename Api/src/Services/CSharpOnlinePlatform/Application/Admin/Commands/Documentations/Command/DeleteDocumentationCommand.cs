using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Admin.Commands.Documentations.Command
{
    public class DeleteDocumentationCommand : IRequest<Guid>
    {
        public Guid Id { get; set; }

        public DeleteDocumentationCommand(Guid docGuid)
        {
            Id = docGuid;
        }
    }

    public class DeleteDocumentationCommandHandler : IRequestHandler<DeleteDocumentationCommand, Guid>
    {
        private readonly IApplicationDbContext _dbContext;
        public DeleteDocumentationCommandHandler(IApplicationDbContext context)
        {
            _dbContext = context;
        }

        public async Task<Guid> Handle(DeleteDocumentationCommand request, CancellationToken cancellationToken)
        {
            var documentation = await _dbContext.GetEntities<Documentation>().SingleAsync(t => t.Id == request.Id);
            _dbContext.Delete(documentation);
            await _dbContext.SaveChangesAsync(cancellationToken);
            return documentation.Id;
        }
    }
}
