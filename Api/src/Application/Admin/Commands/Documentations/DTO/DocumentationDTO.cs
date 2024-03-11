using Domain.Entities;

namespace Application.Documentations.DTO
{
    public class DocumentationDTO : IMapFrom<Documentation>
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
    }
}
