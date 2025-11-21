using Cine_Ma.Models;
using System.ComponentModel.DataAnnotations;

namespace Cine_Ma.ViewModels.Orders
{
    public class OrderCreateViewModel
    {
        public int? OrderId { get; set; }
        [Required]
        public int SessionId { get; set; }
        public int SessionPrice { get; set; }

        [Required]
        public int CinemaId { get; set; }

        [Required]
        public int RoomId { get; set; }

        [Required]
        public int ClientId { get; set; }

        public string MovieTitle { get; set; } = string.Empty;
        // adicionado
        public string MovieDescription { get; set; } = string.Empty;
        public int MovieAge { get; set; }
        public string MovieImage { get; set; } = string.Empty;
        public int MovieDuration { get; set; }
        public DateTime SessionHour { get; set; }
        public string RoomDescription { get; set; } = string.Empty;

        public int BaseTicketPrice { get; set; }

        public List<SeatSelectionViewModel> Seats { get; set; } = [];

        public List<ProductSelectionViewModel> Products { get; set; } = [];

        public List<Client> Clients { get; set; } = [];

        [Required]
        public string PaymentType { get; set; } = "PIX";

        public int TotalPrice { get; set; }
    }

    public class SeatSelectionViewModel
    {
        public int RoomId { get; set; }

        public int Column { get; set; }
        public string Row { get; set; } = string.Empty;

        public bool IsVip { get; set; }

        public bool IsOccupied { get; set; }

        public bool Selected { get; set; }

        public bool HalfPrice { get; set; }

        public int CalculatedPrice { get; set; }
    }

    public class ProductSelectionViewModel
    {
        [Required]
        public int ProductId { get; set; }

        public string Name { get; set; } = string.Empty;

        public int UnitPrice { get; set; }
        public int Discount { get; set; }

        public int Quantity { get; set; }

        public int TotalPrice => (UnitPrice - Discount) * Quantity;
    }
}
