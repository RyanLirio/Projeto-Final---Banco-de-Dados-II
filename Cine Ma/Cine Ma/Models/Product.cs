using System.ComponentModel.DataAnnotations;

namespace Cine_Ma.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public float Price { get; set; }
        public float Discount { get; set; }
        public string UnitMeasure { get; set; }
    }
}
