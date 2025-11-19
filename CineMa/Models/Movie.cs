using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cine_Ma.Models
{
    public class Movie
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string? Title { get; set; }
        [Required]
        public int Duration { get; set; }
        [Required]
        public int MinimumAge { get; set; }
        public float Rating { get; set; }
        public string? Description { get; set; }
        public string? Synopsis { get; set; }

        //[ForeignKey(nameof(IdStudio))]
        //public int IdStudio { get; set; }

        public string? Studio { get; set; }

        [Required]
        public DateOnly DtRelease { get; set; }

        [Required]
        public int LanguageId { get; set; }
        [ForeignKey(nameof(LanguageId))]
        public Language? Language { get; set; }

        public ICollection<SexMovie> SexMovies { get; set; } = new List<SexMovie>();
        public string? ImageUrl { get; set; }
        public bool NowShowing { get; set; }
    }
}
