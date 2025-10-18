using System.ComponentModel.DataAnnotations.Schema;

namespace Cine_Ma.Models
{
    public class Caption
    {
        public int Id { get; set; }
        public int LanguageId { get; set; }
        [ForeignKey(nameof(LanguageId))]
        public Language? Language { get; set; }
    }
}
