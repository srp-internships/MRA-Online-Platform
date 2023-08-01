using Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Admin.Queries
{
    public class GetAdminQuery : IRequest<Domain.Entities.Admin>
    {
        public Guid AdminId { get; set; }
        public GetAdminQuery(Guid adminId)
        {
            AdminId = adminId;
        }
    }

    public class GetAdminQueryHandler : IRequestHandler<GetAdminQuery, Domain.Entities.Admin>
    {
        private readonly IApplicationDbContext _dbcontext;

        public GetAdminQueryHandler(IApplicationDbContext dbContext)
        {
            _dbcontext = dbContext;
        }

        public async Task<Domain.Entities.Admin> Handle(GetAdminQuery request, CancellationToken cancellationToken)
        {
            var admin = _dbcontext.GetEntities<Domain.Entities.Admin>().AsNoTracking()
                .Include(s => s.Contact)
                .FirstOrDefaultAsync(s => s.Id == request.AdminId, cancellationToken);
            return await admin;
        }
    }
}
