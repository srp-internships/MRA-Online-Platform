namespace Domain.Entities
{
    public class StudentCourseExercise : StudentCourseExerciseBase, IEntity
    {
        public virtual Exercise Exercise { get; set; }
        public Guid ExerciseId { get; set; }
        public string Code { get; set; }
    }
    public enum Status
    {
        NotSubmitted,
        Passed,
        Failed,
        WaitForTeacher,
        WaitForStudent
    }
}
