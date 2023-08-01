using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;

namespace Application.Teachers.Commands.ProjectExerciseCommand.DeleteProjectExercise
{
    public class DeleteProjectExerciseCommand : IRequest<Guid>
    {
        public DeleteProjectExerciseCommand(Guid projectExerciseID)
        {
            ProjectExerciseID = projectExerciseID;
        }

        public Guid ProjectExerciseID { get; set; }
    }

    public class DeleteProjectExerciseCommandHandler : IRequestHandler<DeleteProjectExerciseCommand, Guid>
    {
        private readonly IApplicationDbContext _context;
        public DeleteProjectExerciseCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Guid> Handle(DeleteProjectExerciseCommand request, CancellationToken cancellationToken)
        {
            var projectExercise = await _context.GetEntities<ProjectExercise>().FindAsync(request.ProjectExerciseID);
            _context.Delete(projectExercise);
            await _context.SaveChangesAsync(cancellationToken);
            return projectExercise.Id;
        }
    }
}
