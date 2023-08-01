using AutoMapper;
using Domain.Entities;

namespace Application.Teachers.Commands.ProjectExerciseCommand.CheckProjectExercise
{
    public class CheckProjectExerciseCommandDTO
    {
        public Guid ProjectExerciseId { get; set; }
        public Status Status { get; set; }
        public string Comment { get; set; }
    }
}
