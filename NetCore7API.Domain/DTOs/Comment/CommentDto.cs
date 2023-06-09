﻿using NetCore7API.Domain.DTOs.Interfaces;
using NetCore7API.Domain.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetCore7API.Domain.DTOs.Comment
{
    public class CommentDto : BaseDto
    {
        public string Text { get; internal set; }

        public string? FullName { get; internal set; }

        public Guid PostId { get; internal set; }
    }
}