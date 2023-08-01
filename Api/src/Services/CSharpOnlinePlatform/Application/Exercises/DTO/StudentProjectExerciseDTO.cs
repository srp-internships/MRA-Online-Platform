using AutoMapper;
using Domain.Entities;

namespace Application.Exercises.DTO
{
    public class StudentProjectExerciseDTO : IMapFrom<ProjectExercise>
    {
         public Guid Id { get; init; }
        public string Name { get; init; }
        public string Description { get; init; }
        public string Comment { get; init; }
        public string CommentDate { get; init; }
        public Status Status { get; set; }

        public void Mapping(Profile profile)
        {
            var map = profile.CreateMap<ProjectExercise, StudentProjectExerciseDTO>();
            map.ForMember(s => s.Comment, op => op.MapFrom(x => x.Students.Where(s => s.ProjectExerciseId == x.Id)
                    .OrderByDescending(s => s.Date).FirstOrDefault().Comment));
            map.ForMember(s => s.CommentDate, op => op.MapFrom(x => x.Students.Where(s => s.ProjectExerciseId == x.Id)
                   .OrderByDescending(s => s.Date).FirstOrDefault().Date));
            map.ForMember(s => s.Status, op => op.MapFrom(x => x.Students.Where(s => s.ProjectExerciseId == x.Id)
                    .OrderByDescending(s => s.Status == Status.Passed).FirstOrDefault().Status));
        }
    }
}
