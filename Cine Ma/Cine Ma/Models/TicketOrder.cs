using System.ComponentModel.DataAnnotations.Schema;

namespace Cine_Ma.Models
{
    public class TicketOrder
    {
        public int OrderId { get; set; }
        [ForeignKey(nameof(OrderId))]
        public Order? Order { get; set; }
        public int TicketId { get; set; }
        [ForeignKey(nameof(TicketId))]
        public Ticket? Ticket { get; set; }
        public float Value { get; set; }
        public bool HalfPrice { get; set; }
    }
}
