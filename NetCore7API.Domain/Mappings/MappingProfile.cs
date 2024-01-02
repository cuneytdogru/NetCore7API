using AutoMapper;
using NetCore7API.Domain.DTOs.Comment;
using NetCore7API.Domain.DTOs.Post;
using NetCore7API.Domain.DTOs.User;
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
                .ForMember(d => d.IsLiked, opts => opts.MapFrom(s => s.Likes.Any()))
                .ReverseMap().IgnoreAllPropertiesWithAnInaccessibleSetter();

            CreateMap<Comment, CommentDto>()
                .ReverseMap().IgnoreAllPropertiesWithAnInaccessibleSetter();

            CreateMap<User, UserDto>()
                .ReverseMap().IgnoreAllPropertiesWithAnInaccessibleSetter();

            CreateMap<User, PublicUserDto>()
                .ReverseMap().IgnoreAllPropertiesWithAnInaccessibleSetter();

            CreateMap<User, ProfileDto>()
                .ReverseMap().IgnoreAllPropertiesWithAnInaccessibleSetter();
        }
    }
}