using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace Application.Teachers.Commands.ThemeCommands
{
    public class CreateThemeCommand : IRequest<Guid>
    {
        public string Name { get; }
        public string Content { get; }
        public DateTime StartDate { get; }
        public DateTime EndDate { get; }
        public Guid TeacherId { get; }
        public Guid CourseId { get; }

        public CreateThemeCommand(Guid teacherId, string name, string content, DateTime startDate, DateTime endDate, Guid courseId)
        {
            TeacherId = teacherId;
            Name = name;
            Content = content;
            StartDate = startDate;
            EndDate = endDate;
            CourseId = courseId;
        }

        public override string ToString()
        {
            JsonSerializerOptions jso = new JsonSerializerOptions();
            jso.Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping;
            return JsonSerializer.Serialize((object)this, jso);
        }
    }

    public class CreateThemeCommandHandler : IRequestHandler<CreateThemeCommand, Guid>
    {
        private readonly IApplicationDbContext _dbContext;
        private readonly ILogger<CreateThemeCommand> _logger;
        public CreateThemeCommandHandler(IApplicationDbContext dbContext, ILogger<CreateThemeCommand> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }
        public async Task<Guid> Handle(CreateThemeCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"CSharpOnlinePlatform Request create theme, body: {request.ToString()}");
            var theme = new Theme
            {
                Name = request.Name,
                Content = request.Content,
                StartDate = request.StartDate,
                EndDate = request.EndDate,
                CourseId = request.CourseId
            };
            _dbContext.Add(theme);
            await _dbContext.SaveChangesAsync(cancellationToken);
            return theme.Id;
        }
    }
}
