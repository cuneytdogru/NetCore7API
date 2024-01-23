using NetCore7API.Domain.DTOs.User;

namespace NetCore7API.Domain.Services
{
    public interface IUserService
    {
        Task<Guid> RegisterAsync(RegisterUserDto dto);

        Task UpdateAsync(Guid id, UpdateUserDto dto);

        Task ChangePasswordAsync(Guid id, ChangePasswordDto dto);

        Task DeleteAsync(Guid id);
    }
}