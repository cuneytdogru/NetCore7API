using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetCore7API.Domain.DTOs.Interfaces
{
    public class BaseDto : IDto
    {
        public Guid Id { get; set; }
        public DateTime CreatedDate { get; internal set; }
        public Guid CreatedBy { get; internal set; }
        public DateTime ModifiedDate { get; internal set; }
        public Guid ModifiedBy { get; internal set; }
    }
}