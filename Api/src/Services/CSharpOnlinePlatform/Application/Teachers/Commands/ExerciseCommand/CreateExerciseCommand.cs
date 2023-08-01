using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace Application.Teachers.Commands.ExerciseCommand
{
    public class CreateExerciseCommand : IRequest<Guid>
    {
        public CreateExerciseCommandDTO ExerciseDTO { get; init; }
        public Guid TeacherId { get; init; }

        public CreateExerciseCommand(CreateExerciseCommandDTO exerciseDTO, Guid teacherId)
        {
            ExerciseDTO = exerciseDTO;
            TeacherId = teacherId;
        }

        public override string ToString()
        {
            JsonSerializerOptions jso = new JsonSerializerOptions();
            jso.Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping;
            return JsonSerializer.Serialize((object)this, jso);
        }

    }

    public class CreateExerciseCommandHandler : IRequestHandler<CreateExerciseCommand, Guid>
    {
        private readonly IApplicationDbContext _context;
        private readonly ILogger<CreateExerciseCommand> _logger;

        public CreateExerciseCommandHandler(IApplicationDbContext context, ILogger<CreateExerciseCommand> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Guid> Handle(CreateExerciseCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"CSharpOnlinePlatform Request create exercise, body: {request.ToString()}");
            var exercise = new Exercise
            {
                Id = Guid.NewGuid(),
                Name = request.ExerciseDTO.Name,
                Template = request.ExerciseDTO.Template,
                Test = request.ExerciseDTO.Test,
                Description = request.ExerciseDTO.Description,
                Rating = request.ExerciseDTO.Rating,
                ThemeId = request.ExerciseDTO.ThemeId
            };
            _context.Add(exercise);
            await _context.SaveChangesAsync(cancellationToken);
            return exercise.Id;
        }
    }
}