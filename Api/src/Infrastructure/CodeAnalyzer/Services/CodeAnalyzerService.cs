using Application;
using Application.CodeAnalyzer.DTO;
using Application.CodeAnalyzer.Services;
using Core.Exceptions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Text;

namespace Infrastructure.CodeAnalyzer.Services
{
    public class CodeAnalyzerService : ICodeAnalyzerService
    {
        private const string COMPILER_FAILD_CONNECTION_MESSAGE = "Ошибка при соединение с сервером, попробуйте позже";
        private const string COMPILER_FAILD_RESPONSE_MESSAGE = "Внутренняя ошибка сервера";

        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        private readonly ILogger<CodeAnalyzerService> _logger;

        public CodeAnalyzerService(IHttpClientFactory httpClientFactory, IConfiguration configuration, ILogger<CodeAnalyzerService> logger)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
            _logger = logger;
        }

        public async Task<CodeAnalyzeResultDTO> AnalyzeCode(List<string> codes, VersionDTO version)
        {
            var bodyPayload = GetBodyPayload(codes, version);
            var request = GetHttpRequestMessage(bodyPayload);
            var response = await GetResponseMessageAsync(request);
            return await GetResultFromResponse(response);
        }

        HttpRequestMessage GetHttpRequestMessage(StringContent body)
             => new(HttpMethod.Post, _configuration[ApplicationConstants.CODE_ANALYZE_ROUTE])
             {
                 Content = body
             };

        async Task<HttpResponseMessage> GetResponseMessageAsync(HttpRequestMessage httpRequestMessage)
        {
            var httpClient = _httpClientFactory.CreateClient(ApplicationConstants.COMPILER_CLIENT);
            HttpResponseMessage httpResponseMessage;
            try
            {
                httpResponseMessage = await httpClient.SendAsync(httpRequestMessage);
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex.Message);
                throw new CompilerApiException(COMPILER_FAILD_CONNECTION_MESSAGE);
            }
            if (!httpResponseMessage.IsSuccessStatusCode)
            {
                _logger.LogError(await httpResponseMessage.Content.ReadAsStringAsync());
                throw new CompilerApiException(COMPILER_FAILD_RESPONSE_MESSAGE);
            }
            return httpResponseMessage;
        }

        StringContent GetBodyPayload(List<string> codes, VersionDTO version)
        {
            var json = JsonConvert.SerializeObject(CodeAnalyzerQuery.GetNew(codes, version));
            return new StringContent(json, Encoding.UTF8, ApplicationConstants.STRING_CONTENT);
        }

        async Task<CodeAnalyzeResultDTO> GetResultFromResponse(HttpResponseMessage httpResponseMessage)
        {
            var jsonResponse = await httpResponseMessage.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<CodeAnalyzeResultDTO>(jsonResponse);
        }
    }
}
