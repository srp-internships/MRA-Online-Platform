using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Teachers
{
    public class GetTeacherQuery : IRequest<Teacher>
    {
        public Guid TeacherId { get; }

        public GetTeacherQuery(Guid teacherId)
        {
            TeacherId = teacherId;
        }

    }

    public class GetTeacherQueryHandler : IRequestHandler<GetTeacherQuery, Teacher>
    {
        private readonly IApplicationDbContext _applicationDbContext;

        public GetTeacherQueryHandler(IApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        public async Task<Teacher> Handle(GetTeacherQuery request, CancellationToken cancellationToken)
        {
            var teacher = await _applicationDbContext.GetEntities<Teacher>().AsNoTracking()
                .Include(s => s.Contact)
                .Include(t => t.LeadingCourses)
                .ThenInclude(c => c.Students)
                .ThenInclude(c => c.Exercises)
                .Include(t => t.LeadingCourses)
                .ThenInclude(th => th.Themes)
                .ThenInclude(ex => ex.Exercises)
                .ThenInclude(s => s.Students)
                .FirstOrDefaultAsync(t => t.Id == request.TeacherId, cancellationToken);
            return teacher;
        }
    }
}
