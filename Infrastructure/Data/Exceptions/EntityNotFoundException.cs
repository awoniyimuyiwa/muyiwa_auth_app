using Domain.Generic;
using System;

namespace Infrastructure.Data.Exceptions
{
    class EntityNotFoundException : BaseException
    {
        public EntityNotFoundException(Exception innerException = null) : base("Entity not found", innerException)
        {
            Code = 500;
            LocalizableMessage = "EntityNotFound";
        }
    }
}
