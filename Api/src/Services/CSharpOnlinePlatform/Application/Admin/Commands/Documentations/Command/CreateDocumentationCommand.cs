using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;

namespace Application.Admin.Commands.Documentations.Command
{
    public class CreateDocumentationCommand : IRequest<Guid>
    {
        public DocumentArea Area { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
    }

    public class CreateDocumentationCommandHandler : IRequestHandler<CreateDocumentationCommand, Guid>
    {
        private readonly IApplicationDbContext _context;

        public CreateDocumentationCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Guid> Handle(CreateDocumentationCommand request, CancellationToken cancellationToken)
        {
            var doc = new Documentation
            {
                Area = request.Area,
                Title = request.Title,
                Content = request.Content
            };
            _context.Add(doc);
            await _context.SaveChangesAsync(cancellationToken);
            return doc.Id;
        }
    }
}
