using Application.Common.Interfaces;
using Application.Themes.DTO;
using AutoMapper;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Students.Queries
{
    public class GetThemeQuery : IRequest<ThemeDTO>
    {
        public Guid ThemeGuid { get; }

        public Guid StudentGuid { get; }

        public GetThemeQuery(Guid themeGuid, Guid studentGuid)
        {
            ThemeGuid = themeGuid;
            StudentGuid = studentGuid;
        }
    }

    public class GetByGuidThemeQueryHandler : IRequestHandler<GetThemeQuery, ThemeDTO>
    {
        private readonly IApplicationDbContext _dbContext;
        private readonly IMapper _mapper;

        public GetByGuidThemeQueryHandler(IApplicationDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<ThemeDTO> Handle(GetThemeQuery request, CancellationToken cancellationToken)
        {
            var theme = await _dbContext.GetEntities<Theme>().AsNoTracking()
                .Include(th => th.Exercises)
                .Include(s => s.Tests)
                .Include(s => s.ProjectExercises)
                .FirstOrDefaultAsync(s => s.Id == request.ThemeGuid && s.StartDate.Date <= DateTime.Today, cancellationToken);
            return _mapper.Map<ThemeDTO>(theme);
        }
    }
}