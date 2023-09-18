using AutoMapper;
using Domain.Entities;

namespace Application.Teachers.Queries.StudentCourseProjectExerciseQuery;

public class GetStudentCourseProjectExerciseDTO:IMapFrom<StudentCourseProjectExercise>
{
    public Guid Id { get; set; }
    public string LinkToProject { get; set; }
    public Status Status { get; set; }
    public DateTime Date { get; set; }

    public void Mappind(Profile profile)
    {
        profile.CreateMap<StudentCourseProjectExercise,GetStudentCourseProjectExerciseDTO>();
    }
}