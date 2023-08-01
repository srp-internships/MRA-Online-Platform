using Application.Common.Interfaces;
using Application.Courses.DTO;
using AutoMapper;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Account.Commands
{
    public class GetShortCourseCommand : IRequest<List<ShortCourseDTO>>
    {

    }

    public class GetShortCourseCommandHandler : IRequestHandler<GetShortCourseCommand, List<ShortCourseDTO>>
    {
        private readonly IApplicationDbContext _dbContext;
        private readonly IMapper _mapper;

        public GetShortCourseCommandHandler(IApplicationDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<List<ShortCourseDTO>> Handle(GetShortCourseCommand request, CancellationToken cancellationToken)
        {
            var courses = await _dbContext.GetEntities<Course>().AsNoTracking()
                .ToListAsync(cancellationToken: cancellationToken);
            return _mapper.Map<List<ShortCourseDTO>>(courses);
        }
    }
}
