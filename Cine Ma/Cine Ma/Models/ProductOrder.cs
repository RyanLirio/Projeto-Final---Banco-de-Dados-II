using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Net.Sockets;

namespace Cine_Ma.Models
{
    [PrimaryKey(nameof(IdSale), nameof(IdProduct))]
    public class ProductOrder
    {
        [Key]
        public int IdSale { get; set; }
        [ForeignKey(nameof(IdSale))]
        
        [Key]
        public int IdProduct { get; set; }
        [ForeignKey(nameof(IdProduct))]

        public int QuantitySale { get; set; }
    }
}
