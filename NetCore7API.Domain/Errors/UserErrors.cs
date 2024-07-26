using NetCore7API.Domain.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetCore7API.Domain.Errors
{
    public static class UserErrors
    {
        /// <summary>
        /// Username already in use! Please type another username.
        /// </summary>
        public static readonly Error UsernameInUse = new Error(
        "Users.UsernameInUse", "Username already in use! Please type another username.");

        /// <summary>
        /// Email address already in use! Please type another email.
        /// </summary>
        public static readonly Error EmailInUse = new Error(
        "Users.EmailInUse", "Email address already in use! Please type another email.");
    }
}