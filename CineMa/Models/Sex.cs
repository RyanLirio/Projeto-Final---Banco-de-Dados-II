using System.ComponentModel.DataAnnotations;

namespace Cine_Ma.Models
{
    public class Sex
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string? Name { get; set; }
        public ICollection<SexMovie>? SexMovies { get; set; }
    }
}
