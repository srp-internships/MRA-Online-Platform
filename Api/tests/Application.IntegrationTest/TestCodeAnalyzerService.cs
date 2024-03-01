using Application.CodeAnalyzer.DTO;
using Application.CodeAnalyzer.Services;
using Core.Exceptions;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.IntegrationTest
{
    public class TestCodeAnalyzerService : ICodeAnalyzerService
    {
        public const string SuccessCode = "Success code";
        public const string FailCode = "Fail code";
        public const string ExceptionCode = "Exception code";

        public Task<CodeAnalyzeResultDTO> AnalyzeCode(List<string> codes, VersionDTO version)
        {
            if (codes.Any(s => s == ExceptionCode))
                throw new CompilerApiException($"Exception codes: {codes}");
            var success = codes.Any(s => s == SuccessCode) && !codes.Any(s => s == FailCode);
            return Task.FromResult(new CodeAnalyzeResultDTO { Success = success });
        }
    }
}
