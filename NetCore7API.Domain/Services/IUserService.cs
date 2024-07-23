using NetCore7API.Domain.DTOs.User;

namespace NetCore7API.Domain.Services
{
    public interface IUserService
    {
        Task<Guid> RegisterAsync(RegisterUserRequestDto dto);

        Task UpdateAsync(Guid id, UpdateUserRequestDto dto);

        Task ChangePasswordAsync(Guid id, ChangePasswordRequestDto dto);

        Task DeleteAsync(Guid id);
    }
}