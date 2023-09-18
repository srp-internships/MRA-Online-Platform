using Application.Common.BL;
using Application.Common.Interfaces;
using Application.Courses.DTO;
using AutoMapper;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Students.Queries
{
    public class GetCoursesQuery : IRequest<List<CourseDTO>>
    {
        public Guid StudentId { get; }

        public GetCoursesQuery(Guid studentId)
        {
            StudentId = studentId;
        }
    }

    public class GetCoursesQueryHandler : IRequestHandler<GetCoursesQuery, List<CourseDTO>>
    {
        private readonly IApplicationDbContext _dbContext;
        private readonly IMapper _mapper;

        public GetCoursesQueryHandler(IApplicationDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<List<CourseDTO>> Handle(GetCoursesQuery request, CancellationToken cancellationToken)
        {
            var studentGuid = request.StudentId;
            var courses = await _dbContext.GetEntities<StudentCourse>().AsNoTracking()
                           .Include(s => s.Course.Themes).ThenInclude(s => s.Exercises).ThenInclude(s => s.Students.Where(s => s.StudentCourse.StudentId == request.StudentId))
                           .Include(s => s.Course.Themes).ThenInclude(s => s.Tests).ThenInclude(s => s.Students.Where(s => s.StudentCourse.StudentId == request.StudentId))
                           .Where(s => s.StudentId == studentGuid)
                           .Select(st => st.Course)
                           .ToListAsync(cancellationToken: cancellationToken);
            var coursesDtos = MapCourseDTO(courses, studentGuid);
            return coursesDtos;
        }

        List<CourseDTO> MapCourseDTO(List<Course> courses, Guid studentId)
        {
            List<CourseDTO> coursesDtos = new();
            foreach (var course in courses)
            {
                var courseDTO = _mapper.Map<CourseDTO>(course);
                courseDTO.CompletedThemes += course.GetCompletedThemesCount() + course.GetExpiredPassedThemesCount();
                coursesDtos.Add(courseDTO);
            }
            return coursesDtos;
        }
    }
}
