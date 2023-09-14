using Application.Common.Interfaces;
using Application.Documentations.DTO;
using AutoMapper;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Documentations
{
    public class GetDocumentationQuery : IRequest<DocumentationDTO>
    {
        public DocumentArea Area { get; }

        public GetDocumentationQuery(DocumentArea area)
        {
            Area = area;
        }
    }

    public class GetDocumentationQueryHandler : IRequestHandler<GetDocumentationQuery, DocumentationDTO>
    {
        private readonly IApplicationDbContext _dbContext;
        private readonly IMapper _mapper;

        public GetDocumentationQueryHandler(IApplicationDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<DocumentationDTO> Handle(GetDocumentationQuery request, CancellationToken cancellation)
        {
            return await _dbContext.GetEntities<Documentation>().AsNoTracking()
                .Where(s => s.Area == request.Area)
                .Select(s => _mapper.Map<DocumentationDTO>(s))
                .FirstOrDefaultAsync(cancellation);
        }
    }
}
