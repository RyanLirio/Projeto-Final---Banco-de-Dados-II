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

        public int MovieId { get; set; }
        [ForeignKey(nameof(MovieId))]
        public Movie? Movie { get; set; }

        public DateOnly SessionHour { get; set; }

        public int LanguageId { get; set; }
        [ForeignKey(nameof(LanguageId))]
        public Language? LanguageAudio { get; set; }

        public int RoomId { get; set; }
        [ForeignKey(nameof(RoomId))]
        public CinemaRoom? CinemaRoom { get; set; }

        public int? CaptionId { get; set; }
        [ForeignKey(nameof(CaptionId))]
        public Language? LanguageCaption { get; set; }

        public bool Is3D { get; set; }
    } 
}
