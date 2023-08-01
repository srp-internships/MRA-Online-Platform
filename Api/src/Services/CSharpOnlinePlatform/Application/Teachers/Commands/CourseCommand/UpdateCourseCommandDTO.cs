namespace Application.Teachers.Commands.CourseCommand
{
    public record UpdateCourseCommandDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string CourseLanguage { get; set; }
    }
}
