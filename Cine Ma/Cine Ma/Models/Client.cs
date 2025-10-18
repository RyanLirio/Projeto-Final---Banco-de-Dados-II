using System.ComponentModel.DataAnnotations;

namespace Cine_Ma.Models
{
    public class Client
    {
        [Key]
        public int Id { get; set; }
        public string? Cpf {  get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public DateOnly Birthday { get; set; }
        public DateTime? RegistrationDate { get; set; }
    }
}
