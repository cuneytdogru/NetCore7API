using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetCore7API.Domain.Exceptions
{
    public class UserException : Exception
    {
        public UserException(string message) : base(message)
        {
        }

        public UserException(string message, Exception innerException)
         : base(message, innerException) { }
    }
}