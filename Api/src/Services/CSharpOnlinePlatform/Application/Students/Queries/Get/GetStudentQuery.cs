using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Students
{
    public class GetStudentQuery : IRequest<Student>
    {
        public Guid StudentGuid { get; }

        public GetStudentQuery(Guid studentGuid)
        {
            StudentGuid = studentGuid;
        }
    }

    public class GetStudentQueryHandler : IRequestHandler<GetStudentQuery, Student>
    {
        private readonly IApplicationDbContext _applicationDbContext;

        public GetStudentQueryHandler(IApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        public async Task<Student> Handle(GetStudentQuery request, CancellationToken cancellationToken)
        {
            var studentCourse = _applicationDbContext.GetEntities<Student>().AsNoTracking()
                .Include(s => s.Contact)
                .Include(st => st.Courses)
                .ThenInclude(sc => sc.Course)
                .ThenInclude(c => c.Themes)
                .ThenInclude(th => th.Exercises)
                .ThenInclude(st => st.Students)
                .FirstOrDefaultAsync(st => st.Id == request.StudentGuid);
            return await studentCourse;
        }
    }
}
