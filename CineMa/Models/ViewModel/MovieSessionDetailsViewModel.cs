using Cine_Ma.Models;
using System.ComponentModel.DataAnnotations;

namespace CineMa.Models.ViewModel
{
    public class MovieSessionDetailsViewModel
    {
        public String? SelectedCity { get; set; }
        public DateOnly SelectedDay { get; set; }
        public List<Session> Sessions { get; set; } = new();
        public List<DateOnly> DaysWithSession { get; set; } = new();
        public required Movie Movie { get; set; }
        
    }
}
