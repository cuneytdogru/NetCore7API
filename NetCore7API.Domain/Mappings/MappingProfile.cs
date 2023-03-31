using AutoMapper;
using NetCore7API.Domain.DTOs.Comment;
using NetCore7API.Domain.DTOs.Post;
using NetCore7API.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace NetCore7API.Domain.Mappings
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<Post, PostDto>()
                .ReverseMap().IgnoreAllPropertiesWithAnInaccessibleSetter();

            CreateMap<Comment, CommentDto>()
                .ReverseMap().IgnoreAllPropertiesWithAnInaccessibleSetter();
        }
    }
}