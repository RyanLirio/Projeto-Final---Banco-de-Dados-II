using Cine_Ma.Models;
using System.ComponentModel.DataAnnotations;

namespace Cine_Ma.ViewModels.Movies
{
    public class MovieViewModel
    {
        public int? Id { get; set; } 

        [Required]
        public string? Title { get; set; }

        [Required]
        public int Duration { get; set; }

        [Required]
        public int MinimumAge { get; set; }

        public float Rating { get; set; }

        public string? Description { get; set; }
        public string? Synopsis { get; set; }
        public string? Studio { get; set; }

        [Required]
        public DateOnly DtRelease { get; set; }

        [Required]
        public int LanguageId { get; set; }

        public List<Language> Languages { get; set; } = [];

        public List<SelectedSex> Sexes { get; set; } = [];

        public void SetSexes(List<Sex> sexes)
        {
            Sexes = [
                ..sexes.Select(s => new SelectedSex
                {
                    Id = s.Id,
                    Name = s.Name!,
                    IsSelected = false
                })
            ];
        }
    }

    public class SelectedSex
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public bool IsSelected { get; set; }
    }
}
