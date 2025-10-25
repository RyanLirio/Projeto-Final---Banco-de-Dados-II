using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cine_Ma.Models
{
    [PrimaryKey(nameof(IdUnit), nameof(IdClient))]
    public class Order
    {
        public int Id { get; set; }

        [ForeignKey(nameof(IdUnit))]
        public int IdUnit { get; set; }
        public DateOnly DtSale { get; set; }
        public float TotalPrice { get; set; }
        public String PaymentType { get; set; }

        [ForeignKey(nameof(IdClient))]
        public int IdClient { get; set; }
        
    }
}
