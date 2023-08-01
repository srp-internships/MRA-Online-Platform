using Application.CodeAnalyzer.DTO;
using Application.CodeAnalyzer.Services;
using Application.Common.Interfaces;
using Core.Exceptions;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace Application.CodeAnalyzer.Commands
{
    public class AnalyzeCodeCommand : IRequest<CodeAnalyzeResultDTO>
    {
        public AnalyzeCodeCommand(Guid studentGuid, AnalyzeCodeCommandParameter analyzeCodeCommandParameter, VersionDTO versionDTO)
        {
            Parameters = analyzeCodeCommandParameter;
            StudentGuid = studentGuid;
            Version = versionDTO;
        }

        public AnalyzeCodeCommandParameter Parameters { get; }
        public Guid StudentGuid { get; }
        public VersionDTO Version { get; }
        public override string ToString()
        {
            JsonSerializerOptions jso = new JsonSerializerOptions();
            jso.Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping;
            return JsonSerializer.Serialize((object)this, jso);
        }

    }

    public class AnalyzeCodeCommandHandler : IRequestHandler<AnalyzeCodeCommand, CodeAnalyzeResultDTO>
    {
        ICodeAnalyzerService _codeAnalyzerService;
        IApplicationDbContext _applicationDbContext;
        ILogger<AnalyzeCodeCommand> _logger;

        public AnalyzeCodeCommandHandler(ICodeAnalyzerService codeAnalyzerService, IApplicationDbContext applicationDbContext, ILogger<AnalyzeCodeCommand> logger)
        {
            _codeAnalyzerService = codeAnalyzerService;
            _applicationDbContext = applicationDbContext;
            _logger = logger;
        }

        public async Task<CodeAnalyzeResultDTO> Handle(AnalyzeCodeCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"CSharpOnlinePlatform Request analyze code: {request}");
            var exersize = await _applicationDbContext.GetEntities<Exercise>().Include(s => s.Theme).ThenInclude(s => s.Course).SingleAsync(s => s.Id == request.Parameters.Id);
            List<string> codes = new()
            {
                request.Parameters.Code,
                exersize.Test
            };

            try
            {
                CodeAnalyzeResultDTO result = await _codeAnalyzerService.AnalyzeCode(codes, request.Version);
                if (result.InternalError)
                    throw new CompilerApiException(result.Errors);

                Status status = result.Success ? Status.Passed : Status.Failed;
                await CreateNewStudentCourseExercise(exersize, request.Parameters.Code, status, request.StudentGuid, exersize.Theme.Course.Id);
                return result;
            }
            catch (CompilerApiException ex)
            {
                return new CodeAnalyzeResultDTO()
                {
                    Success = false,
                    Errors = ex.Message,
                    InternalError = true
                };
            }
        }

        async Task CreateNewStudentCourseExercise(Exercise exercise, string code, Status status, Guid studentId, Guid courseId)
        {
            var existingStudentCourse = await _applicationDbContext.GetEntities<StudentCourse>().FirstOrDefaultAsync(s => s.StudentId == studentId && s.CourseId == courseId);
            if (existingStudentCourse != null)
            {
                var newStudentCourseExercise = new StudentCourseExercise
                {
                    ExerciseId = exercise.Id,
                    StudentCourseId = existingStudentCourse.Id,
                    Status = status,
                    Code = code,
                    Date = DateTime.Today
                };

                _applicationDbContext.Add(newStudentCourseExercise);
                await _applicationDbContext.SaveChangesAsync();
            }
        }
    }
}
