using Domain.Entities;

namespace Application.Themes.DTO
{
    public record ShortExerciseDTO : IMapFrom<Exercise>
    {
        public Guid Id { get; init; }
        public string Name { get; init; }
    }
}
