using Domain.Entities;

namespace Application.Courses.DTO
{
    public class ShortCourseDTO : IMapFrom<Course>
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }
}
