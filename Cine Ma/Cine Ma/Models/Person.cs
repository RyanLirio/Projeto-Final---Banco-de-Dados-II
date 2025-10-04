namespace Cine_Ma.Models
{
    public class Person
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Gender { get; set; }
        public DateOnly Birthday { get; set; }
        public string Nationality { get; set; }

    }
}
