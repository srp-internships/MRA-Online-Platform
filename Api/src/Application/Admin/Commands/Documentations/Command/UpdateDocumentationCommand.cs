using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Admin.Commands.Documentations.Command
{
    public class UpdateDocumentationCommand : IRequest<Guid>
    {
        public Guid Id { get; set; }
        public DocumentArea Area { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
    }

    public class UpdateDocumentationCommandHandler : IRequestHandler<UpdateDocumentationCommand, Guid>
    {
        private readonly IApplicationDbContext _dbContext;

        public UpdateDocumentationCommandHandler(IApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Guid> Handle(UpdateDocumentationCommand command, CancellationToken cancellationToken)
        {
            var doc = await _dbContext.GetEntities<Documentation>().SingleAsync(s => s.Id == command.Id);
            doc.Area = command.Area;
            doc.Title = command.Title;
            doc.Content = command.Content;
            await _dbContext.SaveChangesAsync(cancellationToken);
            return doc.Id;
        }
    }
}
