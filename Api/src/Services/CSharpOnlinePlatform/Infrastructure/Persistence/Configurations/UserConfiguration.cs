namespace Infrastructure.Persistence.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.Property(s => s.FirstName).IsRequired();
            builder.Property(s => s.LastName).IsRequired();
            builder.Property(s => s.Birthdate).IsRequired();
            builder.Ignore(s => s.Password);

            builder.HasOne(u => u.Contact)
                .WithOne(c => c.User)
                .HasForeignKey<Contact>(u => u.UserId);
        }
    }
}
