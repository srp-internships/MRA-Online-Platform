namespace Domain.Entities
{
    public class Teacher : User
    {
        public virtual ICollection<Course> LeadingCourses { get; set; }
    }
}
