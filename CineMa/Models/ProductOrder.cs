using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Net.Sockets;

namespace Cine_Ma.Models
{
    public class ProductOrder
    {
        [Key]
        public int OrderId { get; set; }
        [ForeignKey(nameof(OrderId))]
        public Order? Order { get; set; }
        
        [Key]
        public int ProductId { get; set; }
        [ForeignKey(nameof(ProductId))]
        public Product? Product { get; set; }

        [Required]
        public int QuantitySale { get; set; }
        [Required]
        public int OrderPrice { get; set; }
    }
}
