using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cine_Ma.Models
{
    public class CinemaRoom
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int CinemaId { get; set; }
        [ForeignKey(nameof(CinemaId))]
        public Cinema? Cinema { get; set; }
        [Required]
        public string? RoomNumber { get; set; }
        public ICollection<Chair>? Chairs { get; set; } = new List<Chair>();
    }
}
