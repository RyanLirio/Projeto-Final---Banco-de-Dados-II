using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cine_Ma.Models
{
    [PrimaryKey(nameof(ActorId), nameof(MovieId))]
    public class ActorMovie
    {
        public int ActorId { get; set; }
        [ForeignKey(nameof(ActorId))]
        public Person? Person { get; set; }
        public int MovieId { get; set; }
        [ForeignKey(nameof(MovieId))]
        public Movie? Movie { get; set; }
    }
}
