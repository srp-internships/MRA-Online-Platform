using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace Application.Teachers.Commands.CourseCommand
{
    public class DeleteCourseCommand : IRequest<Guid>
    {
        public Guid CourseId { get; }
        public Guid TeacherId { get; }

        public override string ToString()
        {
            JsonSerializerOptions jso = new JsonSerializerOptions();
            jso.Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping;
            return JsonSerializer.Serialize((object)this, jso);
        }

        public DeleteCourseCommand(Guid teacherId, Guid courseGuid)
        {
            TeacherId = teacherId;
            CourseId = courseGuid;
        }
    }

    public class DeleteCourseCommandHandler : IRequestHandler<DeleteCourseCommand, Guid>
    {
        private readonly IApplicationDbContext _context;
        private readonly ILogger<DeleteCourseCommand> _logger;

        public DeleteCourseCommandHandler(IApplicationDbContext context, ILogger<DeleteCourseCommand> logger)
        {
            _context = context;
            _logger = logger;
        }
        public async Task<Guid> Handle(DeleteCourseCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"CSharpOnlinePlatform Request delete cource, body: {request.ToString()}");
            var course = await _context.GetEntities<Course>().FindAsync(request.CourseId);
            _context.Delete(course);
            await _context.SaveChangesAsync(cancellationToken);
            return course.Id;
        }
    }
}
