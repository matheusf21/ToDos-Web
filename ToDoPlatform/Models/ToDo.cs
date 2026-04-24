using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ToDoPlatform.Models;

namespace ToDoPlatform.Models;

[Table("todos")]
public class ToDo
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required]
    public string UserId { get; set; }

    [ForeignKey("UserId")]
    [Display(Name = "Usuário")]
    public AppUser User { get; set; }

    [StringLength(100)]
    [Display(Name = "Título")]
    public string Title { get; set; }

    [Display(Name = "Descrição")]
    public string Description { get; set; }

    [Display(Name = "Concluído")]
    public bool Done { get; set; } = false;

    [Display(Name = "Data de cadastro")]
    public DateTime CreatedAt { get; set; } = DateTime.Now;
}