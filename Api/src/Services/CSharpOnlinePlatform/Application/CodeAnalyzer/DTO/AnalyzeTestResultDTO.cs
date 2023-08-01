using AutoMapper;
using Domain.Entities;

namespace Application.CodeAnalyzer.DTO
{
    public class AnalyzeTestResultDTO : IMapFrom<StudentCourseTest>
    {
        public bool Success { get; init; }

        public void Mapping(Profile profile)
        {
            var map = profile.CreateMap<StudentCourseTest, AnalyzeTestResultDTO>();
            map.ForMember(s => s.Success, op => op.MapFrom(x => x.Status == Status.Passed ? true : false));
        }
    }
}
