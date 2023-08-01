using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace Application.Teachers.Commands.ThemeCommands
{
    public class DeleteThemeCommand : IRequest<Guid>
    {
        public Guid ThemeId { get; }
        public Guid TeacherId { get; }

        public DeleteThemeCommand(Guid themeId, Guid teacherId)
        {
            ThemeId = themeId;
            TeacherId = teacherId;
        }

        public override string ToString()
        {
            JsonSerializerOptions jso = new JsonSerializerOptions();
            jso.Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping;
            return JsonSerializer.Serialize((object)this, jso);
        }
    }

    public class DeleteThemeCommandHandler : IRequestHandler<DeleteThemeCommand, Guid>
    {
        private readonly IApplicationDbContext _context;
        private readonly ILogger<DeleteThemeCommand> _logger;
        public DeleteThemeCommandHandler(IApplicationDbContext context, ILogger<DeleteThemeCommand> logger)
        {
            _context = context;
            _logger = logger;
        }
        public async Task<Guid> Handle(DeleteThemeCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"CSharpOnlinePlatform Request delete theme, body: {request.ToString()}");
            var theme = await _context.GetEntities<Theme>().FindAsync(request.ThemeId);
            _context.Delete(theme);
            await _context.SaveChangesAsync(cancellationToken);
            return theme.Id;
        }
    }
}
