using NetTopologySuite.Geometries;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PeliculasAPI2.Entidades
{
    public class Cines
    {
        public int Id { get; set; }
        [Required]
        [StringLength(maximumLength: 75)]
        public string Nombre { get; set; }
        public Point Ubicacion { get; set; } // representa punto en el planeta tierra (latitud longitud)
        public List<PeliculasCines> PeliculasCines { get; set; }
    }
}
