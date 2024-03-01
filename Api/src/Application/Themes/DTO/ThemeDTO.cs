using AutoMapper;
using Domain.Entities;

namespace Application.Themes.DTO
{
    public record ThemeDTO : IMapFrom<Theme>
    {
        public Guid Id { get; init; }
        public string Name { get; init; }
        public string Content { get; init; }
        public DateTime StartDate { get; init; }
        public DateTime EndDate { get; init; }
        public Guid CourseId { get; set; }
        public bool HaveTests { get; init; }
        public bool HaveExercises { get; init; }
        public bool HaveProjectExercise { get; init; }

        public void Mapping(Profile profile)
        {
            var map = profile.CreateMap<Theme, ThemeDTO>();
            map.ForMember(x => x.HaveExercises, op => op.MapFrom(s => s.Exercises.Any()));
            map.ForMember(x => x.HaveTests, op => op.MapFrom(s => s.Tests.Any()));
            map.ForMember(s => s.HaveProjectExercise, op => op.MapFrom(s => s.ProjectExercises.Any()));
        }
    }
}
