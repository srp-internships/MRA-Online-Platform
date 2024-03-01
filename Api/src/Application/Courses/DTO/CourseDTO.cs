using Application.Common.BL;
using AutoMapper;
using Domain.Entities;

namespace Application.Courses.DTO
{
    public record CourseDTO : IMapFrom<Course>
    {
        public Guid Id { get; init; }
        public string Name { get; init; }
        public int TotalThemes { get; init; }
        public int CompletedThemes { get; set; }
        public DateTime EndDate { get; init; }

        public void Mapping(Profile profile)
        {
            var map = profile.CreateMap<Course, CourseDTO>();
            map.ForMember(s => s.TotalThemes, op => op.MapFrom(x => x.Themes.Count));
            map.ForMember(s => s.CompletedThemes, op => op.MapFrom(x => 0));
            map.ForMember(s => s.EndDate, op => op.MapFrom(x => x.GetEndDate()));
        }
    }
}
