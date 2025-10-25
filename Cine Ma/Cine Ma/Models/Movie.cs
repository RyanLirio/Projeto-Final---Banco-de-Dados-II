using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cine_Ma.Models
{
    [PrimaryKey(nameof(IdStudio), nameof(IdLanguage))]
    public class Movie
    {
        public int Id { get; set; }
        public string Title{ get; set; }
        public int Duration { get; set; }
        public int MinimumAge { get; set; }
        public float Rating { get; set; }
        public string Description { get; set; }
        public string Synopsis { get; set; }

        [ForeignKey(nameof(IdStudio))]
        public int IdStudio { get; set; }
        
        public DateOnly DtRelease { get; set; }
        
        [ForeignKey(nameof(IdLanguage))]
        public int IdLanguage { get; set; }
        
        public float Invoicing { get; set; }

    }
}
