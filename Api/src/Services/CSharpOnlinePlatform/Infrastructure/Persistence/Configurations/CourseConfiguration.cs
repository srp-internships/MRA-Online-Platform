namespace Infrastructure.Persistence.Configurations
{
    public class CourseConfiguration : IEntityTypeConfiguration<Course>
    {
        public void Configure(EntityTypeBuilder<Course> builder)
        {
            builder.HasKey(c => c.Id);
            builder.Property(c => c.Id).ValueGeneratedOnAdd();
            builder.Property(c => c.Name).IsRequired();
            builder.Property(c => c.LearningLanguage).IsRequired();

            builder.HasMany(c => c.Students)
                .WithOne(sc => sc.Course)
                .HasForeignKey(sc => sc.CourseId);

            builder.HasOne(c => c.Teacher)
                .WithMany(t => t.LeadingCourses)
                .HasForeignKey(c => c.TeacherId);

            builder.HasMany(c => c.Themes)
                .WithOne(th => th.Course)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
