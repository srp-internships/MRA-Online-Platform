namespace Infrastructure.Persistence.Configurations
{
    public class TestConfiguration : IEntityTypeConfiguration<Test>
    {
        public void Configure(EntityTypeBuilder<Test> builder)
        {
            builder.HasKey(t => t.Id);
            builder.Property(t => t.Id).ValueGeneratedOnAdd();
            builder.Property(t => t.Name).IsRequired();
            builder.Property(t => t.Description).IsRequired();
            builder.Property(t => t.Rating).IsRequired();

            builder.HasOne(t => t.Theme)
                .WithMany(th => th.Tests)
                .HasForeignKey(t => t.ThemeId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(t => t.Students)
                .WithOne(sct => sct.Test)
                .HasForeignKey(sct => sct.TestId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
