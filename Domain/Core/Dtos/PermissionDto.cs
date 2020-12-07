using Domain.Generic;

namespace Domain.Core.Dtos
{
    public record PermissionDto : BaseDto
    {
        public string Name { get; init; }
        public string Description { get; init; }
    }
}
