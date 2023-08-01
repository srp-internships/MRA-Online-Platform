using Application.Account.Services;
using Application.Common.Interfaces;
using Core.Exceptions;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Text;
using System.Text.Json;

namespace Application.Account.Commands
{
    public class SignUpCommand : IRequest<bool>
    {
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string Email { get; set; }
        public string Occupation { get; set; }
        public string Password { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string PhoneNumber { get; set; }
        public string Country { get; set; }
        public string Region { get; set; }
        public string City { get; set; }
        public string Address { get; set; }
        public Guid CourseId { get; set; }

        public override string ToString()
        {
            JsonSerializerOptions jso = new JsonSerializerOptions();
            jso.Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping;
            return JsonSerializer.Serialize((object)this, jso);
        }
    }

    public class SignUpCommandHandler : IRequestHandler<SignUpCommand, bool>
    {
        readonly IApplicationDbContext _dbContext;
        readonly IEmailSenderService _emailService;
        readonly ILogger<SignUpCommand> _logger;
        readonly UserManager<User> _userManager;
        readonly IConfiguration _configuration;

        public SignUpCommandHandler(IApplicationDbContext dbContext, IEmailSenderService emailService, ILogger<SignUpCommand> logger, UserManager<User> userManager, IConfiguration configuration)
        {
            _dbContext = dbContext;
            _emailService = emailService;
            _logger = logger;
            _userManager = userManager;
            _configuration = configuration;
        }

        public async Task<bool> Handle(SignUpCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Sign up student: {request.ToString()}");
            var newStudent = new Student()
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                Birthdate = request.DateOfBirth,
                Occupation = request.Occupation,
                UserName = request.Email,
                Contact = new Contact()
                {
                    PhoneNumber = request.PhoneNumber,
                    Country = request.Country,
                    Region = request.Region,
                    City = request.City,
                    Address = request.Address,
                },
                IsPasswordChanged = true,
            };
            var selectedStudentCourse = await _dbContext.GetEntities<Course>().SingleAsync(c => c.Id == request.CourseId);
            newStudent.Courses.Add(new StudentCourse()
            {
                Course = selectedStudentCourse
            });
            var result = await _userManager.CreateAsync(newStudent, request.Password);
            if (!result.Succeeded)
            {
                var errorResponse = new ErrorResponse(result.Errors.Select(s => (s.Description, s.Code)));
                throw new ValidationFailureException(errorResponse);
            }
            await _userManager.AddToRoleAsync(newStudent, "student");
            var confirmToken = await _userManager.GenerateEmailConfirmationTokenAsync(newStudent);

            var subject = _configuration[ApplicationConstants.SIGN_UP_SUBJECT];
            var body = GetBody(newStudent.Email, confirmToken);
            bool isSendEmail = _emailService.SendEmail(request.Email, body, subject);
            if (!isSendEmail)
            {
                newStudent.EmailConfirmed = true;
                await _userManager.UpdateAsync(newStudent);
            }
            return isSendEmail;
        }

        string GetBody(string emailTo, string resetToken)
        {
            byte[] tokenGeneratedBytes = Encoding.UTF8.GetBytes(resetToken);
            var codeEncoded = WebEncoders.Base64UrlEncode(tokenGeneratedBytes);
            var link = $"{_configuration[ApplicationConstants.SIGN_UP_LINK]}?token={codeEncoded}&email={emailTo}";
            return $"<html><head></head><body>{_configuration[ApplicationConstants.SIGN_UP_MESSAGE]}</br><a href=\"{link}\">Подтвердить</a></body></html>";
        }
    }
}
