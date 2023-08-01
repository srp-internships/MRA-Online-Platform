namespace Infrastructure.Persistence.Configurations
{
    public class StudentConfiguration : IEntityTypeConfiguration<Student>
    {
        public void Configure(EntityTypeBuilder<Student> builder)
        {
            builder.Property(s => s.Occupation).IsRequired();

            builder.HasMany(s => s.Courses)
                .WithOne(sc => sc.Student)
                .HasForeignKey(sc=>sc.StudentId);
        }
    }
}
