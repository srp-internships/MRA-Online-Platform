namespace Application.CodeAnalyzer.DTO
{
    public record CodeAnalyzeResultDTO
    {
        public bool Success { get; init; }
        public string Errors { get; init; }
        public bool InternalError { get; init; }
    }
}
