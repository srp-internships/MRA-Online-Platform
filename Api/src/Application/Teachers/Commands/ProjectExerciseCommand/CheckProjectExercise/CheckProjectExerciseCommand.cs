using Application.Common.Interfaces;
using AutoMapper;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace Application.Teachers.Commands.ProjectExerciseCommand.CheckProjectExercise
{
    public class CheckProjectExerciseCommand : IRequest<Guid>
    {
        public Guid ProjectExerciseId { get; init; }
        public Status Status { get; init; }
        public string Comment { get; init; }
        public Guid TeacherId { get; init; }

        public CheckProjectExerciseCommand(CheckProjectExerciseCommandDTO checkProjectExercise,  Guid teacherId)
        {
            TeacherId = teacherId;
            ProjectExerciseId = checkProjectExercise.ProjectExerciseId;
            Status = checkProjectExercise.Status;
            Comment = checkProjectExercise.Comment;
        }

        public override string ToString()
        {
            JsonSerializerOptions jso = new JsonSerializerOptions();
            jso.Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping;
            return JsonSerializer.Serialize((object)this, jso);
        }
    }

    public class ProjectExerciseResultCommandHandler : IRequestHandler<CheckProjectExerciseCommand, Guid>
    {
        private readonly IApplicationDbContext _dbContext;
        private readonly ILogger<CheckProjectExerciseCommand> _logger;
        private readonly IMapper _mapper;

        public ProjectExerciseResultCommandHandler(IApplicationDbContext _dbContext, ILogger<CheckProjectExerciseCommand> logger, IMapper mapper)
        {
            this._dbContext = _dbContext;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<Guid> Handle(CheckProjectExerciseCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"CSharpOnlinePlatform Request result of project, body: {request.ToString()}");

            var studentCourseProjectExercise = await _dbContext.GetEntities<StudentCourseProjectExercise>()
                .Where(s => s.ProjectExerciseId == request.ProjectExerciseId).SingleAsync();

            studentCourseProjectExercise.Status = request.Status;
            studentCourseProjectExercise.Comment = request.Comment;
            await _dbContext.SaveChangesAsync(cancellationToken);

            return studentCourseProjectExercise.Id;
        }
    }
}
