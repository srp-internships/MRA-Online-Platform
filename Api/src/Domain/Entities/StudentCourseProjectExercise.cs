namespace Domain.Entities
{
    public class StudentCourseProjectExercise : StudentCourseExerciseBase, IEntity
    {
        public virtual ProjectExercise ProjectExercise { get; set; }
        public Guid ProjectExerciseId { get; set; }        
        public string Comment { get; set; }
        public string LinkToProject { get; set; }
    }
}
