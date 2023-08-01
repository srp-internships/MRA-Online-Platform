using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;

namespace Application.Teachers.Commands.TestCommand
{
    public class UpdateTestCommand : IRequest<Guid>
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int Rating { get; set; }
        public string Description { get; set; }
    }

    public class UpdateTestCommandHandler : IRequestHandler<UpdateTestCommand, Guid>
    {
        private readonly IApplicationDbContext _context;

        public UpdateTestCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Guid> Handle(UpdateTestCommand request, CancellationToken cancellationToken)
        {
            var test = await _context.GetEntities<Test>().FindAsync(request.Id);
            test.Name = request.Name;
            test.Rating = request.Rating;
            test.Description = request.Description;
            await _context.SaveChangesAsync(cancellationToken);
            return test.Id;
        }
    }
}
