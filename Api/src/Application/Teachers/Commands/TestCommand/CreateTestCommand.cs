using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace Application.Teachers.Commands.TestCommand
{
    public class CreateTestCommand : IRequest<Guid>
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int Rating { get; set; }
        public Guid ThemeId { get; set; }
    }

    public class CreateTestCommandHandler : IRequestHandler<CreateTestCommand, Guid>
    {
        private readonly IApplicationDbContext _context;

        public CreateTestCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Guid> Handle(CreateTestCommand request, CancellationToken cancellationToken)
        {
            var test = new Test()
            {
                Name = request.Name,
                Description = request.Description,
                Rating = request.Rating,
                ThemeId = request.ThemeId
            };
            _context.Add(test);
            await _context.SaveChangesAsync(cancellationToken);
            return test.Id;
        }
    }
}
