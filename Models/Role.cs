using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Kruggers_Backend.Models;
[Table("Roles")]
public class Role
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Key]

    public int Id { get; set; }


    [Required] [Column("Name")] 
    public string RoleType { get; set; }


    public List<User> Users { get; set; } = new List<User>();
}