using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace Application.Teachers.Commands.ThemeCommands
{
    public class UpdateThemeCommand : IRequest<Guid>
    {
        public Guid ThemeId { get; }
        public string Name { get; }
        public string Content { get; }
        public DateTime StartDate { get; }
        public DateTime EndDate { get; }
        public Guid TeacherId { get; }

        public UpdateThemeCommand(Guid themeId, string name, string content, DateTime startDate, DateTime endDate, Guid teacherId)
        {
            ThemeId = themeId;
            Name = name;
            Content = content;
            StartDate = startDate;
            EndDate = endDate;
            TeacherId = teacherId;
        }

        public override string ToString()
        {
            JsonSerializerOptions jso = new JsonSerializerOptions();
            jso.Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping;
            return JsonSerializer.Serialize((object)this, jso);
        }
    }

    public class UpdateThemeCommandHandler : IRequestHandler<UpdateThemeCommand, Guid>
    {
        private readonly IApplicationDbContext _dbContext;
        private readonly ILogger<UpdateThemeCommand> _logger;

        public UpdateThemeCommandHandler(IApplicationDbContext dbContext, ILogger<UpdateThemeCommand> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task<Guid> Handle(UpdateThemeCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"CSharpOnlinePlatform Request update theme, body: {request.ToString()}");
            var theme = await _dbContext.GetEntities<Theme>().FindAsync(request.ThemeId);
            theme.Name = request.Name;
            theme.Content = request.Content;
            theme.StartDate = request.StartDate;
            theme.EndDate = request.EndDate;
            await _dbContext.SaveChangesAsync(cancellationToken);
            return theme.Id;
        }
    }
}
