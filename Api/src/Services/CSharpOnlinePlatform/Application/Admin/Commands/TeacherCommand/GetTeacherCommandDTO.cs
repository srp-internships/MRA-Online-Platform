using AutoMapper;
using Domain.Entities;

namespace Application.Admin.Commands.TeacherCommand
{
    public class GetTeacherCommandDTO : IMapFrom<Teacher>
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Country { get; set; }
        public string Region { get; set; }
        public string City { get; set; }
        public string Address { get; set; }

        public void Mapping(Profile profile)
        {
            var map = profile.CreateMap<Teacher, GetTeacherCommandDTO>();
            
            map.ForMember(s => s.DateOfBirth, op => op.MapFrom(x => x.Birthdate));
            map.ForMember(s => s.PhoneNumber, op => op.MapFrom(x => x.Contact.PhoneNumber));
            map.ForMember(s => s.Country, op => op.MapFrom(x => x.Contact.Country));
            map.ForMember(s => s.Region, op => op.MapFrom(x => x.Contact.Region));
            map.ForMember(s => s.City, op => op.MapFrom(x => x.Contact.City));
            map.ForMember(s => s.Address, op => op.MapFrom(x => x.Contact.Address));
        }
    }
}
