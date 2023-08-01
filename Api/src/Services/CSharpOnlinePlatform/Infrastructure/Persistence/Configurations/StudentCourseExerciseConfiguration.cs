namespace Infrastructure.Persistence.Configurations
{
    public class StudentCourseExerciseConfiguration : IEntityTypeConfiguration<StudentCourseExercise>
    {
        public void Configure(EntityTypeBuilder<StudentCourseExercise> builder)
        {
            builder.HasKey(sct => sct.Id);
            builder.Property(sct => sct.Id).ValueGeneratedOnAdd();
            builder.Property(sct => sct.Status).IsRequired();
            builder.Property(sct => sct.Code).IsRequired();

            builder.HasOne(sct => sct.StudentCourse)
                .WithMany(sc => sc.Exercises)
                .HasForeignKey(sct => sct.StudentCourseId);

            builder.HasOne(sct=>sct.Exercise)
                .WithMany(t=>t.Students)
                .HasForeignKey(sct=>sct.ExerciseId);
        }
    }
}
