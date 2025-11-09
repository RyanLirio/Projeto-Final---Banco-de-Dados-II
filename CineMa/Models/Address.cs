using Microsoft.Extensions.Primitives;
using System.ComponentModel.DataAnnotations;

namespace Cine_Ma.Models
{
    public class Address
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string? ZipCode { get; set; }
        [Required]
        public string? City { get; set; }
        [Required]
        public string? Road { get; set; }
        [Required]
        public string? State { get; set; }
        [Required]
        public int Number {  get; set; }
        [Required]
        public string? Descripton { get; set; }
    }
}
