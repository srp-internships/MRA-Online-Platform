using Application.Common.Interfaces;
using Core.ValidationsBehaviours;
using Domain.Entities;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace Application.Documentations
{
    public class GetDocumentationQueryValidator : AbstractValidator<GetDocumentationQuery>
    {
        public GetDocumentationQueryValidator(IApplicationDbContext dbContext)
        {
            RuleFor(s => s.Area).Must(area =>
            {
                return dbContext.GetEntities<Documentation>().Any(s => s.Area == area);
            }).WithMessage($"Документ не найден.");
        }
    }
}
