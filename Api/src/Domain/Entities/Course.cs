namespace Domain.Entities
{
    public class Course : IEntity
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string LearningLanguage { get; set; }
        public Guid TeacherId { get; set; }
        public virtual ICollection<Theme> Themes { get; set; } = new List<Theme>();
        public virtual ICollection<StudentCourse> Students { get; set; } = new List<StudentCourse>();
    }
}
