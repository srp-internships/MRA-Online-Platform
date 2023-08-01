namespace Domain.Entities
{
    public class Theme : IEntity
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public virtual Course Course { get; set; }
        public Guid CourseId { get; set; }
        public string Content { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public virtual ICollection<Exercise> Exercises { get; set; } = new List<Exercise>();
        public virtual ICollection<ProjectExercise> ProjectExercises { get; set; } = new List<ProjectExercise>();
        public virtual ICollection<Test> Tests { get; set; } = new List<Test>();
    }
}
