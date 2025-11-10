namespace Kruggers_Backend.Data;

public class UserDTO
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Username { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string? Phone { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public RoleDTO Role { get; set; } 
    public string? ProfileImage { get; set; }
}