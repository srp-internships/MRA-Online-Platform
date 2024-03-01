namespace Infrastructure.Persistence.SeedData
{
    [AttributeUsage(AttributeTargets.Property)]
    internal class SeedDataPropertyAttribute : Attribute
    {
        public SeedDataPropertyAttribute(Type type, double order)
        {
            Order = order;
            Type = type;
        }

        public double Order { get; init; }
        public Type Type { get; init; }
    }
}