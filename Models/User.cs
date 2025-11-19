using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Kruggers_Backend.Models;
[Table("Users")]
public class User
{
    [Key] public int Id { get; set; }


    [Required] [StringLength(50)] public string Name { get; set; }


    [Required] [StringLength(50)] public string LastName { get; set; }


    [Required] [EmailAddress] public string Email { get; set; }
    [Required] public string Username { get; set; }


    public string? Phone { get; set; }


    [Column(TypeName = "date")]
    public DateTime? DateOfBirth { get; set; }
        
    public string HashedPassword { get; set; }


    public string? ProfileImage { get; set; }
    public string? ProfileImageId { get; set; }

    [ForeignKey("Role")] public int RoleId { get; set; }

    public Role Role { get; set; }


    [InverseProperty("Consumer")] public List<ImageTask> CreatedTasks { get; set; } = new List<ImageTask>();


    [InverseProperty("Creator")] public List<ImageTask> AssignedTasks { get; set; } = new List<ImageTask>();


    [InverseProperty("CreatedBy")] public List<Gallery> CreatedGalleries { get; set; } = new List<Gallery>();

    [InverseProperty("RequestedBy")] public List<Gallery> RequestedGalleries { get; set; } = new List<Gallery>();
}