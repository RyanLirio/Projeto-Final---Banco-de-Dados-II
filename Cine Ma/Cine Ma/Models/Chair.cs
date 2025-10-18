using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cine_Ma.Models
{
    [PrimaryKey(nameof(RoomId), nameof(Column), nameof(Row))]
    public class Chair
    {
        public int RoomId { get; set; }
        [ForeignKey(nameof(RoomId))]
        public CinemaRoom? Room { get; set; }
        public int Column { get; set; }
        public string? Row {  get; set; }
    }
}
