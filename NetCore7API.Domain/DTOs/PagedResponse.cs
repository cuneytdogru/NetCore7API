using NetCore7API.Domain.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetCore7API.Domain.DTOs
{
    public class PagedResponse<TResource, TFilter>
        where TResource : IDto
        where TFilter : BaseFilter
    {
        public PagedResponse(IEnumerable<TResource> data, TFilter filter, int totalCount)
        {
            this.Data = data;
            this.Filter = filter;
            this.DataCount = data.Count();
            this.TotalCount = totalCount;
        }

        public IEnumerable<TResource> Data { get; set; }
        public TFilter Filter { get; set; }
        public int DataCount { get; set; }
        public int TotalCount { get; set; }
    }
}