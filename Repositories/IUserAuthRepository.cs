using Kruggers_Backend.Data.RequestDTOS;
using Kruggers_Backend.Data.ResponseDTOS;

namespace Kruggers_Backend.Repositories;

public interface IUserAuthRepository
{
    Task<bool> CreateUserAsync(UserRegistrationDTO userRegistrationDto);
    Task<UserLoginResponseDTO> LoginUser(UserLoginRequestDTO userLoginDto);
}