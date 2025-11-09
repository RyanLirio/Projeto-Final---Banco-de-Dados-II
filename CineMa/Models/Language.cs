using System.ComponentModel.DataAnnotations;

namespace Cine_Ma.Models
{
    public class Language
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string? Name { get; set; }
    }
}
