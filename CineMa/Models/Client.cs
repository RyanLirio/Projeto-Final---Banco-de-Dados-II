using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cine_Ma.Models
{
    public class Client
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string? Name { get; set; }
        [Required]
        public string? Cpf {  get; set; }
        [Required]
        public string? Email { get; set; }
        [Required]
        public string? Phone { get; set; }
        public string? Role { get; set; } = "User";
        [Required]
        public DateOnly Birthday { get; set; }
        [Required]
        public DateTime? RegistrationDate { get; set; }
        [Required]
        public int AddressId { get; set; }
        [ForeignKey(nameof(AddressId))]
        public Address? Address { get; set; }
        public ICollection<Order> Orders { get; set; } = new List<Order>();
        public string? SenhaHash { get; set; } 

    }
}
