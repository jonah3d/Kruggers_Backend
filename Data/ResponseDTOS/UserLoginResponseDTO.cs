namespace Kruggers_Backend.Data.ResponseDTOS;

public class UserLoginResponseDTO
{
    public UserDTO UserDto { get; set; }
    public string Token { get; set; }
}