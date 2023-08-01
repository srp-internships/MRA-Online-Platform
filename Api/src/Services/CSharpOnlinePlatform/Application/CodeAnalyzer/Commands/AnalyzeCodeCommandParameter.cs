namespace Application.CodeAnalyzer.Commands
{
    public record AnalyzeCodeCommandParameter
    {
        public Guid Id { get; init; }

        public string Code { get; init; }
    }
}
