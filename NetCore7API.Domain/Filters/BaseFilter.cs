using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetCore7API.Domain.Filters
{
    public abstract class BaseFilter : IFilter
    {
        [Range(0, int.MaxValue, ErrorMessage = "Skip cannot be negative.")]
        public int? Skip { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Take must be positive.")]
        public int? Take { get; set; }

        public string? OrderBy { get; set; }
    }
}