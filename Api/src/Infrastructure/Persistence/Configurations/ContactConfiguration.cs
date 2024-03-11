namespace Infrastructure.Persistence.Configurations
{
    public class ContactConfiguration : IEntityTypeConfiguration<Contact>
    {
        public void Configure(EntityTypeBuilder<Contact> builder)
        {
            builder.HasKey(c => c.Id);
            builder.Property(c => c.Id).ValueGeneratedOnAdd();
            builder.Property(c => c.Region).IsRequired();
            builder.Property(c => c.Country).IsRequired();
            builder.Property(c => c.Address).IsRequired();
            builder.Property(c => c.City).IsRequired();
        }
    }
}
 