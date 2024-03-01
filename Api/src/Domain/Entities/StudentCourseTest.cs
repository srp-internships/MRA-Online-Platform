namespace Domain.Entities
{
    public class StudentCourseTest : StudentCourseExerciseBase, IEntity
    {
        public virtual Test Test { get; set; }
        public Guid TestId { get; set; }
        public string Answer { get; set; }
    }
}
