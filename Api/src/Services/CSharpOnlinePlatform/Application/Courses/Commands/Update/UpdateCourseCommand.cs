using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace Application.Teachers.Commands.CourseCommand
{
    public class UpdateCourseCommand : IRequest<Guid>
    {
        public Guid CourseId { get; }
        public string CourseName { get; }
        public string CourseLanguage { get; }
        public Guid TeacherId { get; }

        public override string ToString()
        {
            JsonSerializerOptions jso = new JsonSerializerOptions();
            jso.Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping;
            return JsonSerializer.Serialize((object)this, jso);
        }

        public UpdateCourseCommand(Guid teacherId, Guid courseId, string courseName, string courseLanguage)
        {
            TeacherId = teacherId;
            CourseId = courseId;
            CourseName = courseName;
            CourseLanguage = courseLanguage;
        }
    }

    public class UpdateCourseCommandHandler : IRequestHandler<UpdateCourseCommand, Guid>
    {
        private readonly IApplicationDbContext _dbContext;
        private readonly ILogger<UpdateCourseCommand> _logger;

        public UpdateCourseCommandHandler(IApplicationDbContext dbContext, ILogger<UpdateCourseCommand> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task<Guid> Handle(UpdateCourseCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"CSharpOnlinePlatform Request update cource, body: {request.ToString()}");
            var course = await _dbContext.GetEntities<Course>().FindAsync(request.CourseId);
            course.Name = request.CourseName;
            course.LearningLanguage = request.CourseLanguage;
            await _dbContext.SaveChangesAsync();
            return course.Id;
        }
    }
}
