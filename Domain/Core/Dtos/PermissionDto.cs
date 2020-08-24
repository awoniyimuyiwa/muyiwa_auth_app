using Domain.Generic;

namespace Domain.Core.Dtos
{
    public class PermissionDto : BaseDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
