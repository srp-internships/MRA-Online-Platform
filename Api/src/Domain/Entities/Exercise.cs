namespace Domain.Entities
{
    public class Exercise : ExerciseBase, IEntity
    {
        public string Template { get; set; }
        public string Test { get; set; }
        public virtual ICollection<StudentCourseExercise> Students { get; set; }
    }
}
