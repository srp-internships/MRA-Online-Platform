namespace Infrastructure.Persistence.Configurations
{
    public class TeacherConfiguration : IEntityTypeConfiguration<Teacher>
    {
        public void Configure(EntityTypeBuilder<Teacher> builder)
        {            
            builder.HasMany(t => t.LeadingCourses)
                .WithOne(c => c.Teacher)
                .HasForeignKey(c => c.TeacherId);
        }
    }
}
