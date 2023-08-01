using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace Application.Teachers.Commands.ProjectExerciseCommand.CreateProjectExercise
{
    public class CreateProjectExerciseCommand : IRequest<Guid>
    {
        public CreateProjectExerciseCommandDTO Exercise { get; init; }
        public Guid TeacherId { get; init; }

        public CreateProjectExerciseCommand(Guid teacherId, CreateProjectExerciseCommandDTO exer)
        {
            TeacherId = teacherId;
            Exercise = exer;
        }

        public override string ToString()
        {
            JsonSerializerOptions jso = new JsonSerializerOptions();
            jso.Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping;
            return JsonSerializer.Serialize((object)this, jso);
        }
    }

    public class CreateProjectExerciseCommandHandler : IRequestHandler<CreateProjectExerciseCommand, Guid>
    {
        private readonly IApplicationDbContext _context;
        private readonly ILogger<CreateProjectExerciseCommand> _logger;

        public CreateProjectExerciseCommandHandler(IApplicationDbContext context, ILogger<CreateProjectExerciseCommand> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Guid> Handle(CreateProjectExerciseCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"CSharpOnlinePlatform Request create project exercise, body: {request.ToString()}");
            var projectExercise = new ProjectExercise()
            {
                Description = request.Exercise.Description,
                Name = request.Exercise.Name,
                Rating= request.Exercise.Rating,
                ThemeId= request.Exercise.ThemeId
            };

            _context.Add(projectExercise);
            await _context.SaveChangesAsync(cancellationToken);
            return projectExercise.Id;
        }
    }
}
