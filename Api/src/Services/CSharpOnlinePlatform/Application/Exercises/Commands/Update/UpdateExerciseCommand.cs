using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace Application.Teachers.Commands.ExerciseCommand
{
    public class UpdateExerciseCommand : IRequest<Guid>
    {        
        public UpdateExerciseCommandDTO ExerciseDTO { get; init; }
        public Guid TeacherId { get; init; }

        public UpdateExerciseCommand(UpdateExerciseCommandDTO exerciseDTO, Guid teacherId)
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

    public class UpdateExerciseCommandHandler : IRequestHandler<UpdateExerciseCommand, Guid>
    {
        private readonly IApplicationDbContext _context;
        private readonly ILogger<UpdateExerciseCommand> _logger;

        public UpdateExerciseCommandHandler(IApplicationDbContext context, ILogger<UpdateExerciseCommand> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Guid> Handle(UpdateExerciseCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"CSharpOnlinePlatform Request update exercise, body: {request.ToString()}");
            var newExercise = request.ExerciseDTO;
            var exercise = await _context.GetEntities<Exercise>().FindAsync(newExercise.Id);
            exercise.Name = newExercise.Name;
            exercise.Rating = newExercise.Rating;
            exercise.Description = newExercise.Description;
            exercise.Template = newExercise.Template;
            exercise.Test = newExercise.Test;
            await _context.SaveChangesAsync(cancellationToken);
            return exercise.Id;
        }
    }
}
