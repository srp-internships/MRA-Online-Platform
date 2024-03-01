namespace Domain.Entities
{
    public class VariantTest : IEntity
    {
        public Guid Id { get; set; }
        public virtual Test Test { get; set; }
        public Guid TestId { get; set; }
        public string Value { get; set; }
        public bool IsCorrect { get; set; }
    }
}
