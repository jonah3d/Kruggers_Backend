namespace Kruggers_Backend.Data;

public class UserUpdateDTO
{
    public string Name { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string HasedPassword { get; set; }
    public string Phone { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public string? ProfileImage { get; set; }
    public string UserRole { get; set; }
}