using Domain.Generic;
using System.Collections.Generic;

namespace Domain.Core.Dtos
{
    public record RoleDto : BaseDto
    {
        public string Name { get; init; }
        public string Description { get; init; }
        public List<int> Permissions { get; init; }
    }
}
