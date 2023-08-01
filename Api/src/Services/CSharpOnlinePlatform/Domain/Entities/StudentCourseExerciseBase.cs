namespace Domain.Entities
{
    public class StudentCourseExerciseBase
    {
        public Guid Id { get; set; }
        public virtual StudentCourse StudentCourse { get; set; }
        public Guid StudentCourseId { get; set; }
        public DateTime Date { get; set; }
        public Status Status { get; set; }
    }
}
