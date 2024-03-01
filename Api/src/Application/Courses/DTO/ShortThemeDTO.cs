using AutoMapper;
using Domain.Entities;

namespace Application.Courses.DTO;

public record ShortThemeDTO : IMapFrom<Theme>
{
    public string Name { get; init; }
    public Guid Id { get; init; }
    public DateTime StartDate { get; init; }
    public bool IsStarted { get; init; }

    public void Mapping(Profile profile)
    {
        var map = profile.CreateMap<Theme, ShortThemeDTO>();
        map.ForMember(x => x.IsStarted, op => op.MapFrom(s => s.StartDate.Date <= DateTime.Today));
    }
}