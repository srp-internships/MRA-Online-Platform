namespace Infrastructure.Persistence.Configurations
{
    public class VariantConfiguration : IEntityTypeConfiguration<VariantTest>
    {
        public void Configure(EntityTypeBuilder<VariantTest> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedOnAdd();
            builder.Property(x => x.Value).IsRequired();

            builder.HasOne(x => x.Test)
                 .WithMany(x => x.Variants)
                 .HasForeignKey(x => x.TestId);
        }
    }
}
