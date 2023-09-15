namespace Domain.Entities
{
    public abstract class User : IEntity
    {
        public Guid Id { get; set; }
        
        public bool IsPasswordChanged { get; set; }
        public DateTime Birthdate { get; set; }
        public virtual Contact Contact { get; set; }
    }
}