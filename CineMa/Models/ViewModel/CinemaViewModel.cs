using System.ComponentModel.DataAnnotations;

namespace Cine_Ma.ViewModels.Cinemas
{
    public class CinemaViewModel
    {
        public int? Id { get; set; }

        [Required]
        public string? Name { get; set; }

        [Required]
        public string? Cnpj { get; set; }

        [Required]
        public string? Phone { get; set; }

        [Required]
        public string? Email { get; set; }

        [Required]
        public AddressViewModel Address { get; set; } = new AddressViewModel();
    }

    public class AddressViewModel
    {
        public int? Id { get; set; }

        [Required]
        public string? ZipCode { get; set; }

        [Required]
        public string? City { get; set; }

        [Required]
        public string? Road { get; set; }

        [Required]
        public string? State { get; set; }

        [Required]
        public int Number { get; set; }

        public string? Descripton { get; set; }
    }
}