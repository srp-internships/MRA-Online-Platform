using Domain.Entities;

namespace Application.Exercises.DTO
{
    public class TeacherExerciseDTO : IMapFrom<Exercise>
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Template { get; set; }
        public int Rating { get; set; }
        public string Description { get; set; }
        public string Test { get; set; }
        public Guid ThemeId { get; set; }
    }
}
