using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Net.Sockets;

namespace Cine_Ma.Models
{
    [PrimaryKey(nameof(IdFilm), nameof(IdIdioma), nameof(IdSala), nameof(IdLegenda))]
    public class Session
    {
        [Key]
        public int Id { get; set; }

        public int IdFilm { get; set; }
        [ForeignKey(nameof(IdFilm))]

        public DateOnly SessionHour { get; set; }

        public int IdIdioma { get; set; }
        [ForeignKey(nameof(IdIdioma))]

        public String IdSala { get; set; }
        [ForeignKey(nameof(IdSala))]

        public int IdLegenda { get; set; }
        [ForeignKey(nameof(IdLegenda))]

        public bool _3D { get; set; }//3D, n sei como coloco a variavel
    } 
}
