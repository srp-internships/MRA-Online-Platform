using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Persistence.Configurations
{
    public class ProjectExerciseConfiguration : IEntityTypeConfiguration<ProjectExercise>
    {
        public void Configure(EntityTypeBuilder<ProjectExercise> builder)
        {
            builder.HasKey(p => p.Id);
            builder.Property(p => p.Id).ValueGeneratedOnAdd();

            builder.HasMany(p => p.Students)
                .WithOne(spe => spe.ProjectExercise)
                .HasForeignKey(spe => spe.ProjectExerciseId);
        }
    }
}
