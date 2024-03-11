using Application.Common.Interfaces;
using Application.Exercises.DTO;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Teachers.Commands.TestCommand
{
    public class UpdateVariantCommand : IRequest<Guid>
    {
        public VariantTestDTO[] Variants { get; set; }

        public UpdateVariantCommand(VariantTestDTO[] variants)
        {
            Variants = variants;    
        }
    }

    public class UpdateVariantCommandHandler : IRequestHandler<UpdateVariantCommand, Guid>
    {
        private readonly IApplicationDbContext _context;

        public UpdateVariantCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

       public async Task<Guid> Handle(UpdateVariantCommand request, CancellationToken cancellationToken)
        {
            var variants = await _context.GetEntities<VariantTest>().Where(s => s.TestId == request.Variants[0].TestId).ToListAsync(cancellationToken);
            _context.GetEntities<VariantTest>().RemoveRange(variants);
            await _context.SaveChangesAsync(cancellationToken);

            foreach (var variant in request.Variants)
            {
                _context.Add(new VariantTest()
                {
                    Id = variant.Id,
                    TestId = variant.TestId,
                    Value = variant.Value,
                    IsCorrect= variant.IsCorrect,
                });
            }
            await _context.SaveChangesAsync(cancellationToken);
            return request.Variants[0].TestId;
        }
    }
}
