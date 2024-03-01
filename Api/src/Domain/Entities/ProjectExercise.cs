namespace Domain.Entities
{
    public class ProjectExercise : ExerciseBase, IEntity
    {        
        public virtual ICollection<StudentCourseProjectExercise> Students { get; set; }
    }
}
