using AutoMapper;
using Domain.Entities;

namespace Application.Teachers.Queries.ProjectExerciseQuery
{
    public class GetProjectExerciseQueryDTO : IMapFrom<ProjectExercise>
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int Rating { get; set; }
        public Guid ThemeId { get; set; }
        public string Description { get; set; }
    }
}
