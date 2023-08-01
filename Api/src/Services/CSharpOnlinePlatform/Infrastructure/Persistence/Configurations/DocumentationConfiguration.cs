namespace Infrastructure.Persistence.Configurations
{
    public class DocumentationConfiguration : IEntityTypeConfiguration<Documentation>
    {
        public void Configure(EntityTypeBuilder<Documentation> builder)
        {
            builder.HasKey(s => s.Id);
            builder.Property(s => s.Id).ValueGeneratedOnAdd();
            builder.Property(s => s.Title).IsRequired();
            builder.Property(s => s.Content).IsRequired();
        }
    }
}
