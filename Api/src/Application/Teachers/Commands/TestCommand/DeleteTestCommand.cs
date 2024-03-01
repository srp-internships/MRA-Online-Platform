using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;

namespace Application.Teachers.Commands.TestCommand
{
    public class DeleteTestCommand : IRequest<Guid>
    {
        public DeleteTestCommand(Guid testGuid)
        {
            TestId = testGuid;
        }

        public Guid TestId { get; set; }
    }

    public class DeleteTestCommandHandler : IRequestHandler<DeleteTestCommand, Guid>
    {
        private readonly IApplicationDbContext _context;
        public DeleteTestCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Guid> Handle(DeleteTestCommand request, CancellationToken cancellationToken)
        {
            var test = await _context.GetEntities<Test>().FindAsync(request.TestId);
            _context.Delete(test);
            await _context.SaveChangesAsync(cancellationToken);
            return test.Id;
        }
    }
}
