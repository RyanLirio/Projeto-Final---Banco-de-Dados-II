using System.ComponentModel.DataAnnotations.Schema;

namespace Cine_Ma.Models
{
    public class DirectorMovie
    {
        public int PersonId { get; set; }
        [ForeignKey(nameof(PersonId))]
        public Person? Person { get; set; }
        public int MovieId { get; set; }
        [ForeignKey(nameof(MovieId))]
        public Movie? Movie { get; set; }
    }
}
