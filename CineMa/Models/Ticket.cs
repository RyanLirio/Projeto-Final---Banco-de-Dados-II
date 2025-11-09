using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cine_Ma.Models
{
    public class Ticket
    {
        [Key]
        public int Id { get; set; }

        public int SessionId { get; set; }
        [ForeignKey(nameof(SessionId))]
        public Session? Session { get; set; }

        public int Column { get; set; }
        public string? Row { get; set; }
        public int RoomId { get; set; }
        public Chair? Chair { get; set; }

        public int OrderId { get; set; }
        [ForeignKey(nameof(OrderId))]
        public Order? Order { get; set; }

        public int Price { get; set; }
        public bool HalfPrice { get; set; }
    }
}
