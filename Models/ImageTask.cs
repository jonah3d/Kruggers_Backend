using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Kruggers_Backend.Models;

[Table("Tasks")]
public class ImageTask
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Key]

    public int Id { get; set; }


    [ForeignKey("Consumer")] public int ConsumerId { get; set; }


    [ForeignKey("Creator")] public int CreatorId { get; set; }


    [ForeignKey("Status")] public int StatusId { get; set; }


    [Required] [StringLength(500)] public string Description { get; set; }


    public DateTime CreatedDate { get; set; }


    public DateTime? UpdatedDate { get; set; }


    public string? ImageUrl { get; set; }


    public User Consumer { get; set; }

    public User Creator { get; set; }

    public Status Status { get; set; }


}