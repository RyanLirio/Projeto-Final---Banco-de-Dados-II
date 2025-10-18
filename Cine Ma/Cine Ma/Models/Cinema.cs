using Microsoft.Extensions.Primitives;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cine_Ma.Models
{
    public class Cinema
    {
        [Key]
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Cnpj { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }
        public int AddressId { get; set; }
        [ForeignKey(nameof(AddressId))]
        public Address? Address { get; set; }
    }
}
