using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetCore7API.Domain.Models.Interfaces
{
    public interface IEntity
    {
        public Guid Id { get; set; }
    }
}