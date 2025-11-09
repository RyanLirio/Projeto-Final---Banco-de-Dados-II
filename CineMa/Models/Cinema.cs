using Microsoft.Extensions.Primitives;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cine_Ma.Models
{
    public class Cinema
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string? Name { get; set; }
        [Required]
        public string? Cnpj { get; set; }
        [Required]
        public string? Phone { get; set; }
        [Required]
        public string? Email { get; set; }
        [Required]
        public int AddressId { get; set; }
        [ForeignKey(nameof(AddressId))]
        public Address? Address { get; set; }
        public ICollection<CinemaRoom>? Rooms { get; set; }
    }
}
