using Kruggers_Backend.Data;

namespace Kruggers_Backend.Repositories;

public interface IUserAuthRepository
{
    Task<bool> CreateUserAsync(UserRegistrationDTO userRegistrationDto);
    Task<UserLoginResponseDTO> LoginUser(UserLoginDTO userLoginDto);
}