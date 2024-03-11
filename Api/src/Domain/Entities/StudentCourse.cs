namespace Domain.Entities
{
    public class StudentCourse : IEntity
    {
        public Guid Id { get; set; }
        public Guid StudentId { get; set; }
        public virtual Course Course { get; set; }
        public Guid CourseId { get; set; }
        public virtual ICollection<StudentCourseExercise> Exercises { get; set; }
        public virtual ICollection<StudentCourseTest> Tests { get; set; }
        public virtual ICollection<StudentCourseProjectExercise> Projects { get; set; }
    }
}
