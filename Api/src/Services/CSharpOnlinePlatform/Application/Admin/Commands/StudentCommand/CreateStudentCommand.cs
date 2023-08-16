// using Application.Common.Interfaces;
// using Domain.Entities;
// using MediatR;
// using Microsoft.AspNetCore.Identity;
//
// namespace Application.Admin.Commands.StudentCommand
// {
//     public class CreateStudentCommand : IRequest<IdentityResult>
//     {
//         public string LastName { get; set; }
//         public string FirstName { get; set; }
//         public string Email { get; set; }
//         public string Occupation { get; set; }
//         public string Password { get; set; }
//         public DateTime BirthDate { get; set; }
//         public string PhoneNumber { get; set; }
//         public string Country { get; set; }
//         public string Region { get; set; }
//         public string City { get; set; }
//         public string Address { get; set; }
//         public string CourseName { get; set; }
//     }
//
//     public class CreateStudentCommandHandler : IRequestHandler<CreateStudentCommand, IdentityResult>
//     {
//         private readonly IApplicationDbContext _dbContext;
//         // private readonly UserManager<User> _userManager;
//         public CreateStudentCommandHandler(IApplicationDbContext dbContext)
//         {
//             _dbContext = dbContext;
//             // _userManager = userManager;
//         }
//
//         public async Task<IdentityResult> Handle(CreateStudentCommand request, CancellationToken cancellationToken)
//         {
//             var newStudent = new Student()
//             {
//                 FirstName = request.FirstName,
//                 LastName = request.LastName,
//                 Email = request.Email,
//                 // EmailConfirmed = true,
//                 Birthdate = request.BirthDate,
//                 Occupation = request.Occupation,
//                 // UserName = request.Email,
//                 Contact = new Contact()
//                 {
//                     PhoneNumber = request.PhoneNumber,
//                     Country = request.Country,
//                     Region = request.Region,
//                     City = request.City,
//                     Address = request.Address,
//                 }
//             };
//
//             var selectedStudentCourse = _dbContext.GetEntities<Course>().FirstOrDefault(c => c.Name == request.CourseName);
//             if (selectedStudentCourse != null)
//             {
//                 newStudent.Courses.Add(new StudentCourse()
//                 {
//                     Course = selectedStudentCourse
//                 });
//             }
//
//             // var result = await _userManager.CreateAsync(newStudent, request.Password);
//             // if (result.Succeeded)
//             // {
//                 // result = await _userManager.AddToRoleAsync(newStudent, "student");
//             // }
//             return result;
//         }
//     }
// }
