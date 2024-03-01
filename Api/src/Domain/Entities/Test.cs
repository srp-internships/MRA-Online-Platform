namespace Domain.Entities
{
    public class Test : ExerciseBase, IEntity
    {
        public virtual ICollection<VariantTest> Variants { get; set; }
        public virtual ICollection<StudentCourseTest> Students { get; set; }
    }
}
