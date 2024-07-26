using NetCore7API.Domain.DTOs.User;
using NetCore7API.Domain.Results;

namespace NetCore7API.Domain.Services
{
    public interface IUserService
    {
        Task<IResult<Guid>> RegisterAsync(RegisterUserRequestDto dto);

        Task<IResult> UpdateAsync(Guid id, UpdateUserRequestDto dto);

        Task<IResult> ChangePasswordAsync(Guid id, ChangePasswordRequestDto dto);

        Task<IResult> DeleteAsync(Guid id);
    }
}