namespace Application.Teachers.Commands.ExerciseCommand
{
    public record UpdateExerciseCommandDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int Rating { get; set; }
        public string Template { get; set; }
        public string Test { get; set; }
        public string Description { get; set; }
    }
}
