using Application.CodeAnalyzer.DTO;

namespace Infrastructure.CodeAnalyzer.Services
{
    public record CodeAnalyzerQuery
    {
        public List<string> Codes { get; private init; }

        public VersionDTO DotNetVersionInfo { get; private init; }

        public static CodeAnalyzerQuery GetNew(List<string> codes, VersionDTO version)
        {
            return new CodeAnalyzerQuery()
            {
                Codes = codes,
                DotNetVersionInfo = version
            };
        }
    }
}
