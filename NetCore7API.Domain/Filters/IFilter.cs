using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetCore7API.Domain.Filters
{
    public interface IFilter
    {
        int? Skip { get; set; }
        int? Take { get; set; }
        string? OrderBy { get; set; }
    }
}