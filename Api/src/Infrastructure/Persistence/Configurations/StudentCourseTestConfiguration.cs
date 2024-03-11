namespace Infrastructure.Persistence.Configurations
{
    public class StudentCourseTestConfiguration : IEntityTypeConfiguration<StudentCourseTest>
    {
        public void Configure(EntityTypeBuilder<StudentCourseTest> builder)
        {
            builder.HasKey(sct => sct.Id);
            builder.Property(sct => sct.Id).ValueGeneratedOnAdd();
            builder.Property(sct => sct.Status).IsRequired();

            builder.HasOne(sct => sct.StudentCourse)
                .WithMany(sc => sc.Tests)
                .HasForeignKey(sct => sct.StudentCourseId);

            builder.HasOne(sct => sct.Test)
                .WithMany(t => t.Students)
                .HasForeignKey(sct => sct.TestId);
        }
    }
}
