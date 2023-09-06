using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Admin.Commands.Documentations
{
    public class GetAllDocumentationQuery : IRequest<List<Documentation>>
    {

    }

    public class GetAllDocumentationQueryHandler : IRequestHandler<GetAllDocumentationQuery, List<Documentation>>
    {
        private readonly IApplicationDbContext _dbContext;

        public GetAllDocumentationQueryHandler(IApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<Documentation>> Handle(GetAllDocumentationQuery request, CancellationToken cancellationToken)
        {
            return await _dbContext.GetEntities<Documentation>().AsNoTracking()
                .ToListAsync(cancellationToken);
        }
    }
}
