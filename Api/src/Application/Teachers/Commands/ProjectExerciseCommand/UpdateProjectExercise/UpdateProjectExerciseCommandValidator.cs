using Application.Common.Interfaces;
using Application.Teachers.Commands.TestCommand;
using Core.ValidationsBehaviours;
using Domain.Entities;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Application.Teachers.Commands.ProjectExerciseCommand.UpdateProjectExercise
{
    public class UpdateProjectExerciseCommandValidator : AbstractValidator<UpdateProjectExerciseCommand>
    {
        public UpdateProjectExerciseCommandValidator(IApplicationDbContext dbContext, IConfiguration configuration)
        {
            int minRate = int.Parse(configuration[ApplicationConstants.MIN_RATE_EXERCISE]);
            int maxRate = int.Parse(configuration[ApplicationConstants.MAX_RATE_EXERCISE]);
            RuleFor(c => c.Name).NotEmpty().WithMessage(ValidationMessages.GetNotEmptyMessage("{PropertyName}"));
            RuleFor(c => c.Description).NotEmpty().WithMessage(ValidationMessages.GetNotEmptyMessage("{PropertyName}"));
            RuleFor(c => c.Rating).GreaterThanOrEqualTo(minRate).WithMessage(ValidationMessages.GetGreaterThanOrEqualToMessage("{PropertyName}", minRate))
                                            .LessThanOrEqualTo(maxRate).WithMessage(ValidationMessages.GetLessThanOrEqualToMessage("{PropertyName}", maxRate));

            RuleFor(query => query.Id).Must(proId =>
            {
                return dbContext.GetEntities<ProjectExercise>().Any(t => t.Id == proId);
            }).WithMessage($"Проект не найден.");
        }
    }
}
