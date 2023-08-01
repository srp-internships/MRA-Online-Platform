using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Application.Admin.Commands.TeacherCommand
{
    public class CreateTeacherCommand : IRequest<IdentityResult>
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Country { get; set; }
        public string Region { get; set; }
        public string City { get; set; }
        public string Address { get; set; }
    }

    public class CreateTeacherCommandHandler : IRequestHandler<CreateTeacherCommand, IdentityResult>
    {
        private readonly UserManager<User> _userManager;
        public CreateTeacherCommandHandler(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IdentityResult> Handle(CreateTeacherCommand request, CancellationToken cancellationToken)
        {
            var newTeacher = new Teacher()
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                Birthdate = request.DateOfBirth,
                UserName = request.Email,
                Contact = new Contact()
                {
                    PhoneNumber = request.PhoneNumber,
                    Region = request.Region,
                    Country = request.Country,
                    City = request.City,
                    Address = request.Address,
                }
            };

            var result = await _userManager.CreateAsync(newTeacher, request.Password);
            if (result.Succeeded)
            {
                result = await _userManager.AddToRoleAsync(newTeacher, "teacher");
            }
            return result;
        }
    }
}
