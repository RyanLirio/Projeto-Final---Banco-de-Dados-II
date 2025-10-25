namespace Cine_Ma.Models
{
    public class TicketOrder
    {
        public int Id { get; set; }
        public int TicketId { get; set; }
        public float Value { get; set; }
        public bool HalfPrice { get; set; }
    }
}
