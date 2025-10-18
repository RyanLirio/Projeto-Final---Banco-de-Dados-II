using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cine_Ma.Models
{
    public class CinemaRoom
    {
        [Key]
        public int Id { get; set; }
        public int CinemaId { get; set; }
        [ForeignKey(nameof(CinemaId))]
        public Cinema? Cinema { get; set; }
    }
}
