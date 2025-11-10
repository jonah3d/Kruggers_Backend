using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Kruggers_Backend.Models;
[Table("Statuses")]
public class Status
{
    
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

    public int Id { get; set; }

    [Required] [Column("Name")] 
    public string StatusType { get; set; }

    public List<ImageTask> Tasks { get; set; } = new List<ImageTask>(); 
}