using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ToDoPlatform.Models;

namespace ToDoPlatform.MOdels;

[Table("todos")]
public class ToDoPlatform
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id {get; set;}

    [Required]
    public string UserId {get; set;}
    [ForeignKey(nameof(UserId))]
    public AppUser User { get; set;}

    [StringLength(100)]
    [Display(Name = "Título")]
    public string Title { get; set; }

    public string Description { get; set; }

    public bool Done { get; set; } = false;

    public DateTime CreateAt { get; set; } = DateTime.Now;
}