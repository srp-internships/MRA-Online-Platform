using Application.Common.Interfaces;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace Application.Admin.Queries
{
    public class GetAdminQueryValidator : AbstractValidator<GetAdminQuery>
    {
        public GetAdminQueryValidator(IApplicationDbContext dbContext)
        {
            RuleFor(adminId => adminId.AdminId).Must(adminId =>
            {
                return dbContext.GetEntities<Domain.Entities.Admin>().AsNoTracking().Any(s => s.Id == adminId);
            }).WithMessage("Админ не найден.");
        }
    }
}
