using NetCore7API.Domain.DTOs.Interfaces;
using NetCore7API.Domain.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetCore7API.Domain.DTOs.User
{
    public class ProfileDto : BaseDto
    {
        public string UserName { get; set; }

        public string FullName { get; set; }

        public int TotalPosts { get; set; }
    }
}