using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Admin.Commands.TeacherCommand.TeacherCRUD
{
    public class UpdateTeacherCommand : UpdateTeacherCommandDTO, IRequest<Guid>
    {
    }

    public class UpdateTeacherCommandHandler : IRequestHandler<UpdateTeacherCommand, Guid>
    {      
        private readonly IApplicationDbContext _dbContext;
   
        public UpdateTeacherCommandHandler( IApplicationDbContext dbContext)
        {           
            _dbContext = dbContext;
        }

        public async Task<Guid> Handle(UpdateTeacherCommand request, CancellationToken cancellationToken)
        {
            var teacher = await _dbContext.GetEntities<Teacher>().Include(c => c.Contact).SingleAsync(t => t.Id == request.Id);

            teacher.FirstName = request.FirstName;
            teacher.LastName = request.LastName;
            teacher.Email = request.Email;
            teacher.Birthdate = request.DateOfBirth;
            // teacher.UserName = request.Email;            
            teacher.Contact.Country = request.Country;
            teacher.Contact.Region = request.Region;
            teacher.Contact.City = request.City;
            teacher.Contact.Address = request.Address;
            teacher.Contact.PhoneNumber = request.PhoneNumber;

            await _dbContext.SaveChangesAsync(cancellationToken);
            return teacher.Id;            
        }
    }
}
