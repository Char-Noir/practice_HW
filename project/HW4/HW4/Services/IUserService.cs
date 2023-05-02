using HW4.DTO;

namespace HW4.Services;

public interface IUserService
{
    public Task<UserResponseDto> AddUserAsync(UserRequestDto user);
    public Task<UserResponseDto> GetUserAsync(int id);
}