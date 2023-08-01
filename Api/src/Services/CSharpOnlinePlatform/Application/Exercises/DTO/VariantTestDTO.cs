using Domain.Entities;

namespace Application.Exercises.DTO
{
    public class VariantTestDTO : IMapFrom<VariantTest>
    {
        public Guid Id { get; set; }
        public Guid TestId { get; set; }
        public string Value { get; set; }
        public bool IsCorrect { get; set; }
    }
}
