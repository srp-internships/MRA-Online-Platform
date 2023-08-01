using Application.Common.Interfaces;
using Application.Teachers.DTO;
using AutoMapper;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Teachers.Queries.CourseQuery
{
    public class GetTeacherCoursesQuery : IRequest<List<GetCourseDTO>>
    {
        public Guid TeacherId { get; }

        public GetTeacherCoursesQuery(Guid teacherGuid)
        {
            TeacherId = teacherGuid;
        }
    }
    public class GetCoursesQueryHandler : IRequestHandler<GetTeacherCoursesQuery, List<GetCourseDTO>>
    {
        private readonly IApplicationDbContext _dbContext;
        private readonly IMapper _mapper;

        public GetCoursesQueryHandler(IApplicationDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<List<GetCourseDTO>> Handle(GetTeacherCoursesQuery request, CancellationToken cancellationToken)
        {
            var courses = await _dbContext.GetEntities<Course>().AsNoTracking()
                .Include(t => t.Themes)
                .Where(t => t.TeacherId == request.TeacherId)
                .ToListAsync(cancellationToken: cancellationToken);
            return _mapper.Map<List<GetCourseDTO>>(courses);
        }
    }
}
