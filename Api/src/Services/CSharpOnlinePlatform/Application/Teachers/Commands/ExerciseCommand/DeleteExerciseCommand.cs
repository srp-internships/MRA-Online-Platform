using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace Application.Teachers.Commands.ExerciseCommand
{
    public class DeleteExerciseCommand : IRequest<Guid>
    {
        public Guid ExerciseId { get; }
        public Guid TeacherId { get; }

        public DeleteExerciseCommand(Guid exerciseId, Guid teacherId)
        {
            ExerciseId = exerciseId;
            TeacherId = teacherId;
        }

        public override string ToString()
        {
            JsonSerializerOptions jso = new JsonSerializerOptions();
            jso.Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping;
            return JsonSerializer.Serialize((object)this, jso);
        }

    }

    public class DeleteExerciseCommandHandler : IRequestHandler<DeleteExerciseCommand, Guid>
    {
        private readonly IApplicationDbContext _context;
        private readonly ILogger<DeleteExerciseCommand> _logger;
        public DeleteExerciseCommandHandler(IApplicationDbContext context, ILogger<DeleteExerciseCommand> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Guid> Handle(DeleteExerciseCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"CSharpOnlinePlatform Request delegte exercise, body: {request.ToString()}");
            var exercise = await _context.GetEntities<Exercise>().FindAsync(request.ExerciseId);
            _context.Delete(exercise);
            await _context.SaveChangesAsync(cancellationToken);
            return exercise.Id;
        }
    }
}
