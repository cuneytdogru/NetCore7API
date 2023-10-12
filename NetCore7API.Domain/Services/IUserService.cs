using NetCore7API.Domain.DTOs.User;

namespace NetCore7API.Domain.Services
{
    public interface IUserService
    {
        Task<UserDto> GetAsync(Guid id);

        Task<UserDto> RegisterAsync(RegisterUserDto dto);

        Task<UserDto> UpdateAsync(Guid id, UpdateUserDto dto);

        Task<UserDto> ChangePasswordAsync(Guid id, ChangePasswordDto dto);

        Task<UserDto> DeleteAsync(Guid id);
    }
}