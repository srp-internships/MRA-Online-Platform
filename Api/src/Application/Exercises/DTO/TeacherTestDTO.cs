using Domain.Entities;

namespace Application.Exercises.DTO
{
    public class TeacherTestDTO : IMapFrom<Test>
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int Rating { get; set; }
        public Guid ThemeId { get; set; }
        public string Description { get; set; }
        public virtual ICollection<VariantTestDTO> Variants { get; set; }
    }
}
