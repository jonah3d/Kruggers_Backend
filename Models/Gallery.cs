using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Kruggers_Backend.Models;
[Table("Galleries")]
public class Gallery
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

    public int Id { get; set; }


    [Required] [StringLength(300)] public string ImageUrl { get; set; }


    [Required] [MaxLength(500)] public string Description { get; set; }


    [ForeignKey("CreatedBy")] public int CreatorId { get; set; }

    public User CreatedBy { get; set; }


    [ForeignKey("RequestedBy")] public int RequesterId { get; set; }

    public User RequestedBy { get; set; }
}