namespace Domain.Entities
{
    public class Teacher : User, IEntity
    {
        public virtual ICollection<Course> LeadingCourses { get; set; }
    }
}
