using Microsoft.AspNetCore.Identity;

namespace Domain.Entities
{
    public abstract class User : IdentityUser<Guid>, IEntity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Password { get; set; }
        public bool IsPasswordChanged { get; set; }
        public DateTime Birthdate { get; set; }
        public virtual Contact Contact { get; set; }
        public string FullName => $"{LastName} {FirstName}";
    }
}
