using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Admin.Commands.TeacherCommand.TeacherCRUD
{
    public class DeleteTeacherCommand : IRequest<Guid>
    {
        public Guid TeacherId { get; }

        public DeleteTeacherCommand(Guid teacherId)
        {
            TeacherId = teacherId;
        }
    }

    public class DeleteTeacherCommandHandler : IRequestHandler<DeleteTeacherCommand, Guid>
    {
        private readonly IApplicationDbContext _dbContext;
        public DeleteTeacherCommandHandler(IApplicationDbContext context)
        {
            _dbContext = context;
        }

        public async Task<Guid> Handle(DeleteTeacherCommand request, CancellationToken cancellationToken)
        {
            var teacher = await _dbContext.GetEntities<Teacher>().SingleAsync(t => t.Id == request.TeacherId);
            _dbContext.Delete(teacher);
            await _dbContext.SaveChangesAsync(cancellationToken);
            return teacher.Id;
        }
    }
}
