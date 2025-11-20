namespace Kruggers_Backend.Data.RequestDTOS;

public class UserRegistrationDTO
{
    public string Firstname { get; set; }
    public string Lastname { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
    public string Email { get; set; }
    public string? UserRole { get; set; }
}