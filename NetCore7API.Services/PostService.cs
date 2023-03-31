using AutoMapper;
using NetCore7API.Domain.DTOs.Post;
using NetCore7API.Domain.Exceptions;
using NetCore7API.Domain.Filters;
using NetCore7API.Domain.Models;
using NetCore7API.Domain.Repositories;
using NetCore7API.Domain.Extensions;
using NetCore7API.Domain.DTOs;

namespace NetCore7API.Services
{
    public class PostService : Domain.Services.IPostService
    {
        private readonly IPostRepository _postRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public PostService(
            IPostRepository postRepository,
            IUnitOfWork unitOfWork,
            IMapper mapper
            )
        {
            _postRepository = postRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<PagedResponse<PostDto, PostFilter>> ListAsync(PostFilter filter)
        {
            var response = await _postRepository.ListBlogFeedAsync(filter);

            var totalCount = await _postRepository.TotalCountAsync(filter);

            var mappedResponse = _mapper.Map<IEnumerable<PostDto>>(response);

            return new PagedResponse<PostDto, PostFilter>(mappedResponse, filter, totalCount);
        }

        public async Task<PostDto> GetAsync(Guid id)
        {
            var entity = await _postRepository.GetPostDetailAsync(id);

            if (entity == null)
                throw new UserException("Post not found.");

            return _mapper.Map<PostDto>(entity);
        }

        public async Task<PostDto> CreateAsync(CreatePostDto dto)
        {
            var entity = new Post(dto.Text, dto.ImageURL, dto.FullName);

            _postRepository.Add(entity);

            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<PostDto>(entity);
        }

        public async Task<PostDto> UpdateAsync(Guid id, UpdatePostDto dto)
        {
            var entity = await _postRepository.FindAsync(id);

            if (entity == null)
                throw new UserException("Post not found.");

            entity.Update(dto);

            _postRepository.Update(entity);

            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<PostDto>(entity);
        }

        public async Task<PostDto> LikeAsync(Guid id, LikePostDto dto)
        {
            var entity = await _postRepository.FindAsync(id);

            if (entity == null)
                throw new UserException("Post not found.");

            entity.Like(dto);

            _postRepository.Update(entity);

            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<PostDto>(entity);
        }

        public async Task<PostDto> DeleteAsync(Guid id)
        {
            var entity = await _postRepository.FindAsync(id);

            if (entity == null)
                throw new Exception("Post not found.");

            _postRepository.SoftDelete(entity);

            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<PostDto>(entity);
        }
    }
}