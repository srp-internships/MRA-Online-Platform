using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Teachers.Commands.ProjectExerciseCommand.UpdateProjectExercise
{
    public class UpdateProjectExerciseCommand : IRequest<Guid>
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int Rating { get; set; }
        public string Description { get; set; }
    }

    public class UpdateProjectExerciseCommandHandler : IRequestHandler<UpdateProjectExerciseCommand, Guid>
    {
        private readonly IApplicationDbContext _dbContext;

        public UpdateProjectExerciseCommandHandler(IApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }   

        public async Task<Guid> Handle(UpdateProjectExerciseCommand request, CancellationToken cancellationToken)
        {
            var projectExercise = await _dbContext.GetEntities<ProjectExercise>().SingleAsync(s => s.Id == request.Id);
            projectExercise.Name = request.Name;
            projectExercise.Rating = request.Rating;
            projectExercise.Description = request.Description;
            await _dbContext.SaveChangesAsync(cancellationToken);
            return projectExercise.Id;
        }
    }
}
