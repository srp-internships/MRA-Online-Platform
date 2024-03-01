namespace Infrastructure.Persistence.Configurations
{
    public class StudentCourseProjectExerciseConfiguration : IEntityTypeConfiguration<StudentCourseProjectExercise>
    {
        public void Configure(EntityTypeBuilder<StudentCourseProjectExercise> builder)
        {
            builder.HasKey(p => p.Id);
            builder.Property(p => p.Id).ValueGeneratedOnAdd();

            builder.HasOne(p => p.ProjectExercise)
                .WithMany(spe => spe.Students)
                .HasForeignKey(spe => spe.ProjectExerciseId);

            builder.HasOne(p => p.StudentCourse)
                .WithMany(spe => spe.Projects)
                .HasForeignKey(spe => spe.StudentCourseId);
        }
    }
}
