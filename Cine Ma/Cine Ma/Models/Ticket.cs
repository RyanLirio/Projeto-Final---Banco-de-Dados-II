using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cine_Ma.Models
{
    [PrimaryKey(nameof(IdSessao), nameof(ChairNumber), nameof(Row))]
    public class Ticket
    {
        [Key]
        public int Id { get; set; }

        public int IdSessao { get; set; }
        [ForeignKey(nameof(IdSessao))]

        public int ChairNumber { get; set; }
        [ForeignKey(nameof(ChairNumber))]

        public String? Row { get; set; }
        [ForeignKey(nameof(Row))]

        public Chair? Chair { get; set; }
    }
}
