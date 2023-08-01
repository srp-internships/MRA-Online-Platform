using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace Application.Teachers.Commands.CourseCommand
{
    public class CreateCourseCommand : IRequest<Guid>
    {
        public string Name { get; }
        public string CourseLanguage { get; }
        public Guid TeacherId { get; }

        public CreateCourseCommand(Guid teacherId, string name, string language)
        {
            TeacherId = teacherId;
            Name = name;
            CourseLanguage = language;
        }

        public override string ToString()
        {
            JsonSerializerOptions jso = new JsonSerializerOptions();
            jso.Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping;
            return JsonSerializer.Serialize((object)this, jso);
        }
    }

    public class CreateCourseCommandHandler : IRequestHandler<CreateCourseCommand, Guid>
    {
        private readonly IApplicationDbContext _context;
        private readonly ILogger<CreateCourseCommand> _logger;

        public CreateCourseCommandHandler(IApplicationDbContext context, ILogger<CreateCourseCommand> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Guid> Handle(CreateCourseCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"CSharpOnlinePlatform Request create cource, body: {request.ToString()}");
            var course = new Course
            {
                Name = request.Name,
                LearningLanguage = request.CourseLanguage,
                TeacherId = request.TeacherId,
            };
            _context.Add(course);
            await _context.SaveChangesAsync(cancellationToken);
            return course.Id;
        }
    }
}
