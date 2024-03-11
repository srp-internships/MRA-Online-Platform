using AutoMapper;
using Domain.Entities;

namespace Application.Teachers.DTO
{
    public class GetRatingDTO : IMapFrom<StudentCourse>
    {
        public int TotalRate { get; set; }
        public int TotalSubmit { get; set; }

        public void Mapping(Profile profile)
        {
            var map = profile.CreateMap<StudentCourse, GetRatingDTO>();
            map.ForMember(x => x.TotalRate, op => op.MapFrom(s => (s.Exercises != null ? s.Exercises.Where(s => s.Status == Status.Passed).Sum(s => s.Exercise.Rating) : 0) +
                                                                     (s.Tests != null ? s.Tests.Where(s => s.Status == Status.Passed).Sum(s => s.Test.Rating) : 0)));
            map.ForMember(x => x.TotalSubmit, op => op.MapFrom(s => (s.Exercises != null ? s.Exercises.Count() : 0) + (s.Tests != null ? s.Tests.Count() : 0)));
        }
    }
}