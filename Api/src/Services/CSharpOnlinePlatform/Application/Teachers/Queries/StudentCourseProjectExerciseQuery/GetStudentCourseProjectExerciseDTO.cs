using AutoMapper;
using Domain.Entities;

namespace Application.Teachers.Queries.StudentCourseProjectExerciseQuery
{
    public class GetStudentCourseProjectExerciseDTO : IMapFrom<StudentCourseProjectExercise>
    {
        public Guid Id { get; set; }
        public string LinkToProject { get; set; }
        public string FullName { get; set; }
        public Status Status { get; set; }
        public DateTime Date { get; set; }

        public void Mapping(Profile profile)
        {
            var map = profile.CreateMap<StudentCourseProjectExercise, GetStudentCourseProjectExerciseDTO>();
            // map.ForMember(s => s.FullName, op => op.MapFrom(x => x.StudentCourse.Student.FullName));
        }
    }
}
