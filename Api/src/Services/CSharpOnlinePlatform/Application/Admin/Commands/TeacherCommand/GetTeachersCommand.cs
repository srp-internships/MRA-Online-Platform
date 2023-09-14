using Application.Common.Interfaces;
using AutoMapper;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Admin.Commands.TeacherCommand
{
    public class GetTeachersCommand : IRequest<List<GetTeacherCommandDTO>>
    {

    }

    public class GetTeachersQueryHandler : IRequestHandler<GetTeachersCommand, List<GetTeacherCommandDTO>>
    {
        private readonly IApplicationDbContext _dbContext;
        private readonly IMapper _mapper;

        public GetTeachersQueryHandler(IApplicationDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }
        public async Task<List<GetTeacherCommandDTO>> Handle(GetTeachersCommand request, CancellationToken cancellationToken)
        {
            var teachers = await _dbContext.GetEntities<Teacher>().Include(c => c.Contact).ToListAsync(cancellationToken: cancellationToken);
            var teachersDtos = _mapper.Map<List<GetTeacherCommandDTO>>(teachers);
            return teachersDtos;
        }
    }
}