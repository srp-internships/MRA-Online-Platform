using Application.CodeAnalyzer.DTO;
using Application.Common.Interfaces;
using AutoMapper;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace Application.CodeAnalyzer.Commands
{
    public class AnalyzeTestCommand : IRequest<AnalyzeTestResultDTO>
    {
        public AnalyzeTestCommand(Guid studentId, Guid testId, Guid variantId)
        {
            StudentId = studentId;
            TestId = testId;
            VariantId = variantId;
        }

        public Guid StudentId { get; init; }
        public Guid TestId { get; init; }
        public Guid VariantId { get; init; }

        public override string ToString()
        {
            JsonSerializerOptions jso = new JsonSerializerOptions();
            jso.Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping;
            return JsonSerializer.Serialize((object)this, jso);
        }
    }

    public class AnalyzeTestCommandHandler : IRequestHandler<AnalyzeTestCommand, AnalyzeTestResultDTO>
    {
        IApplicationDbContext _applicationDbContext;
        IMapper _mapper;
        ILogger<AnalyzeTestCommand> _logger;

        public AnalyzeTestCommandHandler(IApplicationDbContext applicationDbContext, IMapper mapper, ILogger<AnalyzeTestCommand> logger)
        {
            _applicationDbContext = applicationDbContext;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<AnalyzeTestResultDTO> Handle(AnalyzeTestCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"CSharpOnlinePlatform Request analyze test: {request}");
            var test = await _applicationDbContext.GetEntities<Test>()
                        .Include(s => s.Variants)
                        .Include(s => s.Theme)
                        .ThenInclude(s => s.Course)
                        .SingleAsync(s => s.Id == request.TestId);

            var result = test.Variants.Single(s => s.Id == request.VariantId).IsCorrect;
            Status status = result ? Status.Passed : Status.Failed;
            return _mapper.Map<AnalyzeTestResultDTO>(await CreateNewStudentCourseExercise(test, test.Variants.Single(s => s.Id == request.VariantId).Value, status, request.StudentId, test.Theme.Course.Id));
        }

        async Task<StudentCourseTest> CreateNewStudentCourseExercise(Test test, string answer, Status status, Guid studentId, Guid courseId)
        {
            var testStudentCourse = await _applicationDbContext.GetEntities<StudentCourse>().FirstOrDefaultAsync(s => s.StudentId == studentId && s.CourseId == courseId);
            if (testStudentCourse != null)
            {
                var newStudentCourseTest = new StudentCourseTest
                {
                    TestId = test.Id,
                    StudentCourseId = testStudentCourse.Id,
                    Status = status,
                    Answer = answer,
                    Date = DateTime.Today
                };
                _applicationDbContext.Add(newStudentCourseTest);
                await _applicationDbContext.SaveChangesAsync();
                return newStudentCourseTest;
            }
            return default;
        }
    }
}
