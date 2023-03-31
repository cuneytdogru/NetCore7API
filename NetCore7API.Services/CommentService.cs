using AutoMapper;
using NetCore7API.Domain.DTOs.Comment;
using NetCore7API.Domain.Exceptions;
using NetCore7API.Domain.Filters;
using NetCore7API.Domain.Models;
using NetCore7API.Domain.Repositories;
using NetCore7API.Domain.DTOs;

namespace NetCore7API.Services
{
    public class CommentService : Domain.Services.ICommentService
    {
        private readonly IRepository<Comment> _commentRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CommentService(
            IRepository<Comment> commentRepository,
            IUnitOfWork unitOfWork,
            IMapper mapper
            )
        {
            _commentRepository = commentRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<PagedResponse<CommentDto, CommentFilter>> ListAsync(CommentFilter filter)
        {
            var response = await _commentRepository.ListAsync(filter);

            var totalCount = await _commentRepository.TotalCountAsync(filter);

            var mappedResponse = _mapper.Map<IEnumerable<CommentDto>>(response);

            return new PagedResponse<CommentDto, CommentFilter>(mappedResponse, filter, totalCount);
        }

        public async Task<CommentDto> GetAsync(Guid id)
        {
            var entity = await _commentRepository.FindAsync(id);

            if (entity == null)
                throw new UserException("Comment not found.");

            return _mapper.Map<CommentDto>(entity);
        }

        public async Task<CommentDto> CreateAsync(CreateCommentDto dto)
        {
            var entity = new Comment(dto.PostId, dto.Text, dto.FullName);

            _commentRepository.Add(entity);

            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<CommentDto>(entity);
        }

        public async Task<CommentDto> UpdateAsync(Guid id, UpdateCommentDto dto)
        {
            var entity = await _commentRepository.FindAsync(id);

            if (entity == null)
                throw new UserException("Comment not found.");

            entity.Update(dto);

            _commentRepository.Update(entity);

            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<CommentDto>(entity);
        }

        public async Task<CommentDto> HideAsync(Guid id, HideCommentDto dto)
        {
            var entity = await _commentRepository.FindAsync(id);

            if (entity == null)
                throw new UserException("Comment not found.");

            entity.Hide(dto);

            _commentRepository.Update(entity);

            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<CommentDto>(entity);
        }

        public async Task<CommentDto> DeleteAsync(Guid id)
        {
            var entity = await _commentRepository.FindAsync(id);

            if (entity == null)
                throw new Exception("Comment not found.");

            _commentRepository.SoftDelete(entity);

            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<CommentDto>(entity);
        }
    }
}