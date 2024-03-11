namespace Infrastructure.Persistence.Configurations
{
    public class StudentCourseConfiguration : IEntityTypeConfiguration<StudentCourse>
    {
        public void Configure(EntityTypeBuilder<StudentCourse> builder)
        {
            builder.HasKey(sc=>sc.Id);
            builder.Property(sc=>sc.Id).ValueGeneratedOnAdd();
            
            builder.HasOne(sc => sc.Course)
                .WithMany(s => s.Students)
                .HasForeignKey(sc => sc.CourseId)
                .OnDelete(DeleteBehavior.ClientCascade);

            builder.HasMany(sc => sc.Projects)
                .WithOne(s => s.StudentCourse)
                .HasForeignKey(sc => sc.StudentCourseId);
        }
    }
}
