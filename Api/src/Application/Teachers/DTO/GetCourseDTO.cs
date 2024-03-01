using Application.Common.BL;
using AutoMapper;
using Domain.Entities;

namespace Application.Teachers.DTO
{
    public class GetCourseDTO : IMapFrom<Course>
    {
        public Guid Id { get; init; }
        public string Name { get; init; }
        public string CourseLanguage { get; set; }
        public int TotalThemes { get; init; }
        public DateTime EndDate { get; init; }

        public void Mapping(Profile profile)
        {
            var map = profile.CreateMap<Course, GetCourseDTO>();
            map.ForMember(s => s.TotalThemes, op => op.MapFrom(x => x.Themes.Count));
            map.ForMember(s => s.CourseLanguage, op => op.MapFrom(x => x.LearningLanguage));
            map.ForMember(s => s.EndDate, op => op.MapFrom(x => x.GetEndDate()));
        }
    }
}
