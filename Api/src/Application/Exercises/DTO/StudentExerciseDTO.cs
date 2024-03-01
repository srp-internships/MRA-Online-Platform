using AutoMapper;
using Domain.Entities;

namespace Application.Exercises.DTO
{
    public record StudentExerciseDTO : IMapFrom<Exercise>
    {
        public Guid Id { get; init; }
        public string Name { get; init; }
        public string Description { get; init; }
        public string Template { get; init; }
        public Status Status { get; set; }

        public void Mapping(Profile profile)
        {
            var map = profile.CreateMap<Exercise, StudentExerciseDTO>();
            map.ForMember(s => s.Template, op => op.MapFrom(x => x.Students.Where(s => s.ExerciseId == x.Id && s.Status == Status.Passed).Any()
                                                    ? x.Students.Single(s => s.ExerciseId == x.Id && s.Status == Status.Passed).Code : x.Template));
            map.ForMember(s => s.Status, op => op.MapFrom(x => x.Students.Where(s => s.ExerciseId == x.Id)
                    .OrderByDescending(s => s.Status == Status.Passed).FirstOrDefault().Status));
        }
    }
}
