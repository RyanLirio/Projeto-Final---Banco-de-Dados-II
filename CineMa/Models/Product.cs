using System.ComponentModel.DataAnnotations;

namespace Cine_Ma.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string? Name { get; set; }
        [Required]
        public int Price { get; set; }
        [Required]
        public int Discount { get; set; }
        [Required]
        public string? Category { get; set; }
        public ICollection<ProductOrder>? ProductOrders { get; set; }
    }
}
