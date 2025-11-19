using System.ComponentModel.DataAnnotations;

namespace Cine_Ma.ViewModels.Rooms
{
    public class CinemaRoomViewModel
    {
        public int? Id { get; set; }

        [Required]
        public int CinemaId { get; set; }

        [Required]
        public string? RoomNumber { get; set; }

        public int TotalColumns { get; set; }
        public int TotalRows { get; set; }

        public List<string> VipRows { get; set; } = new();
        public List<ChairViewModel> Chairs { get; set; } = new();
    }

    public class ChairViewModel
    {
        public int? RoomId { get; set; }

        [Required]
        public int Column { get; set; }

        [Required]
        public string? Row { get; set; }
        public bool IsVip { get; set; }
    }
}
