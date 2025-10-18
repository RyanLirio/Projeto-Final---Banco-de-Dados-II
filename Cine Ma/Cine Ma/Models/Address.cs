using Microsoft.Extensions.Primitives;

namespace Cine_Ma.Models
{
    public class Address
    {
        public int Id { get; set; }
        public string? ZipCode { get; set; }
        public string? City { get; set; }
        public string? Road { get; set; }
        public string? State { get; set; }
        public int Number {  get; set; }
        public string Descripton { get; set; }
    }
}
