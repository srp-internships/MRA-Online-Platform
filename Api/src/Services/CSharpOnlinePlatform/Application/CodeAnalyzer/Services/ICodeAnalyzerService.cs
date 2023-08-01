using Application.CodeAnalyzer.DTO;

namespace Application.CodeAnalyzer.Services
{
    public interface ICodeAnalyzerService
    {
        public Task<CodeAnalyzeResultDTO> AnalyzeCode(List<string> codes, VersionDTO version);
    }
}
