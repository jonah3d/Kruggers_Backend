namespace Kruggers_Backend.Data;

public class UserLoginResponseDTO
{
    public UserDTO UserDto { get; set; }
    public string Token { get; set; }
}