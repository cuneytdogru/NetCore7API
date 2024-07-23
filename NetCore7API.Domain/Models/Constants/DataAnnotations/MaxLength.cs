using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetCore7API.Domain.Models.Constants.DataAnnotations
{
    public class MaxLength
    {
        public const int Default = 255;
        public const int Username = 255;
        public const int Password = 50;
        public const int Text = 4000;
    }
}