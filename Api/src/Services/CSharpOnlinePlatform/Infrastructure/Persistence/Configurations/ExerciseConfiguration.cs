

namespace Infrastructure.Persistence.Configurations
{
    public class ExerciseConfiguration : IEntityTypeConfiguration<Exercise>
    {
        public void Configure(EntityTypeBuilder<Exercise> builder)
        {
            builder.HasKey(t => t.Id);
            builder.Property(t => t.Id).ValueGeneratedOnAdd();
            builder.Property(t => t.Template).IsRequired();
            builder.Property(t => t.Test).IsRequired();
            builder.Property(t => t.Rating).IsRequired();

            builder.HasOne(t => t.Theme)
                .WithMany(th => th.Exercises)
                .HasForeignKey(t => t.ThemeId);

            builder.HasMany(t => t.Students)
                .WithOne(sct => sct.Exercise)
                .HasForeignKey(sct => sct.ExerciseId);
        }
    }
}
