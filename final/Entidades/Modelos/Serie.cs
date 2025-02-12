using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades.Modelos
{
    public class Serie
    {
        public int SerieID { get; set; }
        public string? Nombre { get; set; }
        public DateTime FechaCreacion { get; set; }
        
        // Relación muchos a muchos con Carta
        public ICollection<CartaSerie> CartaSeries { get; set; } = new List<CartaSerie>();


    }
}
