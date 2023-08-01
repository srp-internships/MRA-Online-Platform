namespace Infrastructure.Persistence.Configurations
{
    public class ThemeConfiguration : IEntityTypeConfiguration<Theme>
    {
        public void Configure(EntityTypeBuilder<Theme> builder)
        {
            builder.HasKey(th => th.Id);
            builder.Property(th => th.Id).ValueGeneratedOnAdd();
            builder.Property(th => th.Name).IsRequired();
            builder.Property(th => th.Content).IsRequired();
            builder.Property(th => th.StartDate).IsRequired();
            builder.Property(th => th.EndDate).IsRequired();

            builder.HasOne(th => th.Course)
                .WithMany(c => c.Themes)
                .HasForeignKey(th => th.CourseId);

            builder.HasMany(th => th.Exercises)
                .WithOne(t => t.Theme)
                .HasForeignKey(t => t.ThemeId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(th => th.Tests)
                .WithOne(t => t.Theme)
                .HasForeignKey(t => t.ThemeId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(th => th.ProjectExercises)
                .WithOne(t => t.Theme)
                .HasForeignKey(t => t.ThemeId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
