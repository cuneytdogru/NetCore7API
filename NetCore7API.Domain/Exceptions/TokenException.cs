using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetCore7API.Domain.Exceptions
{
    public class TokenException : Exception
    {
        public TokenException(string message) : base(message)
        {
        }

        public TokenException(string message, Exception innerException)
         : base(message, innerException) { }
    }
}