using System;
using System.Collections.Generic;
using System.Text;

namespace Company.Domain.Entities
{
    public abstract class BaseEntity
    {
        public Guid Id { get; set; }
        public DateTime CreatedTime { get; set; } = DateTime.UtcNow;

    }
}
