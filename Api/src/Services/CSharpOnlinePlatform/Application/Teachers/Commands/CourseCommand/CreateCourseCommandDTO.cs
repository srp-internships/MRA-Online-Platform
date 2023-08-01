namespace Application.Teachers.Commands.CourseCommand
{
    public record CreateCourseCommandDTO
    {
        public string Name { get; set; }
        public string CourseLanguage { get; set; }
    }
}
