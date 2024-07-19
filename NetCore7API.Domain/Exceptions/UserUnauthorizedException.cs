using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetCore7API.Domain.Exceptions
{
    public class UserUnauthorizedException : Exception
    {
        public UserUnauthorizedException(string message) : base(message)
        {
        }

        public UserUnauthorizedException(string message, Exception innerException)
         : base(message, innerException) { }
    }
}