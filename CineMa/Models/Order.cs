using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cine_Ma.Models
{
    public class Order
    {
        [Key]
        public int Id { get; set; }

        public int CinemaId { get; set; }
        [ForeignKey(nameof(CinemaId))]
        public Cinema? Cinema { get; set; }
        public DateOnly DtSale { get; set; }
        public int TotalPrice { get; set; }
        public string? PaymentType { get; set; }

        public int ClientId { get; set; }
        [ForeignKey(nameof(ClientId))]
        public Client? Client { get; set; }

        public ICollection<ProductOrder>? ProductOrders { get; set; }
        public ICollection<Ticket>? Tickets { get; set; }
    }
}
