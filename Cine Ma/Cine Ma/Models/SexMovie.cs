using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;

namespace Cine_Ma.Models
{
    [PrimaryKey(nameof(SexId), nameof(MovieId))]
    public class SexMovie
    {
        public int SexId { get; set; }
        [ForeignKey(nameof(SexId))]
        public Sex? Sex { get; set; }

        public int MovieId { get; set; }
        [ForeignKey(nameof(MovieId))]
        public Movie? Movie { get; set; }
    }
}
