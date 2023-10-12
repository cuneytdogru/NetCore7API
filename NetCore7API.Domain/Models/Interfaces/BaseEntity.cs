using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetCore7API.Domain.Models.Interfaces
{
    public abstract class BaseEntity : IEntity, IAuditedEntity, ISoftDeletedEntity
    {
        public BaseEntity() { 
            this.Id = Guid.NewGuid();
        }

        public Guid Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public Guid CreatedBy { get; set; }
        public DateTime ModifiedDate { get; set; }
        public Guid ModifiedBy { get; set; }
        public bool Deleted { get; set; }
    }
}