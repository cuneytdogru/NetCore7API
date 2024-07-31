using NetCore7API.Domain.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetCore7API.Domain.Errors
{
    public static class LoginErrors
    {
        /// <summary>
        /// Email address already in use! Please type another email.
        /// </summary>
        public static readonly Error InvalidUsernameOrPassword = new Error(
        "Login.InvalidUsernameOrPassword", "Invalid username or password!");
    }
}