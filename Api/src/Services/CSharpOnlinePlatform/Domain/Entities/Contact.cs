namespace Domain.Entities
{
    public class Contact : IEntity
    {
        public Guid Id { get; set; }
        public string PhoneNumber { get; set; }
        public string Region { get; set; }
        public string Country { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public virtual User User { get; set; }
        public Guid UserId { get; set; }
    }
}
