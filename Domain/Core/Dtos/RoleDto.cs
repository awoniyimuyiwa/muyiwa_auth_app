using Domain.Generic;
using System.Collections.Generic;

namespace Domain.Core.Dtos
{
    public class RoleDto : BaseDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public List<int> Permissions { get; set; }
    }
}
