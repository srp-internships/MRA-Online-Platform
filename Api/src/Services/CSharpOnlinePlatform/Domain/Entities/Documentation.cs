namespace Domain.Entities
{
    public class Documentation : IEntity
    {
        public Guid Id { get; set; }
        public DocumentArea Area { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
    }

    public enum DocumentArea
    {
        Admin,
        Teacher,
        Student
    }
}
