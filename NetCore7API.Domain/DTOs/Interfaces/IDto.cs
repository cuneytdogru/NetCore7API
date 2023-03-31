using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetCore7API.Domain.DTOs
{
    public interface IDto
    {
        public Guid Id { get; set; }
        public DateTime CreatedDate { get; }

        public Guid CreatedBy { get; }

        public DateTime ModifiedDate { get; }

        public Guid ModifiedBy { get; }
    }
}