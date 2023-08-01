namespace Application.Teachers.Commands.ThemeCommands
{
    public record CreateThemeCommandDTO
    {
        public string Name { get; set; }
        public string Content { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public Guid CourseId { get; set; }
    }
}
