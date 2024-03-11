using AutoMapper;
using Domain.Entities;

namespace Application.Exercises.DTO
{
    public class StudentTestDTO : IMapFrom<Test>
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string CorrectVariant { get; set; }
        public ICollection<StudentVariantDTO> Variants { get; set; }
        public Status Status { get; set; }

        public void Mapping(Profile profile)
        {
            var map = profile.CreateMap<Test, StudentTestDTO>();
            map.ForMember(s => s.CorrectVariant, op => op.MapFrom(x => x.Students.FirstOrDefault(s => s.TestId == x.Id).Answer));
            map.ForMember(s => s.Status, op => op.MapFrom(x => x.Students.Where(s => s.TestId == x.Id)
                    .OrderByDescending(s => s.Date).FirstOrDefault().Status));
        }
    }
}
