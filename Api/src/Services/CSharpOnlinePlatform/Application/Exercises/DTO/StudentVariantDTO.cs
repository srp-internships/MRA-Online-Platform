using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Exercises.DTO
{
    public class StudentVariantDTO : IMapFrom<VariantTest>
    {
        public Guid Id { get; set; }
        public string Value { get; set; }
    }
}
