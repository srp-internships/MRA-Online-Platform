namespace Application.Teachers.Commands.ProjectExerciseCommand.CreateProjectExercise
{
    public class CreateProjectExerciseCommandDTO
    {
        public string Name { get; set; }
        public int Rating { get; set; }        
        public string Description { get; set; }
        public Guid ThemeId { get; set; }
    }
}
