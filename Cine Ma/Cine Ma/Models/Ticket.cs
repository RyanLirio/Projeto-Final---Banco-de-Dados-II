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

        public int ChairNumber { get; set; }
        [ForeignKey(nameof(ChairNumber))]
        public Chair? ChairNum { get; set; }

        public String? Row { get; set; }
        [ForeignKey(nameof(Row))]

        public Chair? ChairRow { get; set; }
    }
}
