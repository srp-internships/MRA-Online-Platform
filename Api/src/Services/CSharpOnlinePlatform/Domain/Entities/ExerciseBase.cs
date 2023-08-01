namespace Domain.Entities
{
    public class ExerciseBase
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int Rating { get; set; }
        public Guid ThemeId { get; set; }
        public virtual Theme Theme { get; set; }
        public string Description { get; set; }
    }
}
