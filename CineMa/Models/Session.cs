using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Net.Sockets;

namespace Cine_Ma.Models
{
    public class Session
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int MovieId { get; set; }
        [ForeignKey(nameof(MovieId))]
        public Movie? Movie { get; set; }

        [Required]
        public DateTime SessionHour { get; set; }

        [Required]
        public int LanguageId { get; set; }
        [ForeignKey(nameof(LanguageId))]
        public Language? LanguageAudio { get; set; }

        [Required]
        public int RoomId { get; set; }
        [ForeignKey(nameof(RoomId))]
        public CinemaRoom? CinemaRoom { get; set; }

        public int? CaptionId { get; set; }
        [ForeignKey(nameof(CaptionId))]
        public Language? LanguageCaption { get; set; }

        [Required]
        public bool Is3D { get; set; }
        public ICollection<Ticket>? Tickets { get; set; }
    } 
}
