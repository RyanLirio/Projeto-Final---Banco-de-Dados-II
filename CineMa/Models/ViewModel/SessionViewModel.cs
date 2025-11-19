using Cine_Ma.Models;
using System.ComponentModel.DataAnnotations;

namespace Cine_Ma.ViewModels.Sessions
{
    public class SessionViewModel
    {
        public int? Id { get; set; }

        [Required]
        public int MovieId { get; set; }

        [Required]
        public int RoomId { get; set; }

        [Required]
        public int LanguageId { get; set; }

        public int? CaptionId { get; set; }

        [Required]
        public DateTime SessionHour { get; set; }

        [Required]
        public int TicketPrice { get; set; }

        [Required]
        public bool Is3D { get; set; }

        public List<Movie> Movies { get; set; } = new();
        public List<CinemaRoom> Rooms { get; set; } = new();
        public List<Language> AudioLanguages { get; set; } = new();
        public List<Language> CaptionLanguages { get; set; } = new();

    }
}
