using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetCore7API.Domain.Models
{
    public class AppSettings
    {
        public static Authentication Authentication { get; set; }
        public static Password Password { get; set; }
    }

    public class Authentication
    {
        public string Issuer { get; set; } = string.Empty;
        public string Audience { get; set; } = string.Empty;
        public string Key { get; set; } = string.Empty;
    }

    public class Password
    {
        public string Pepper { get; set; } = string.Empty;
        public int Iteration { get; set; }
    }
}