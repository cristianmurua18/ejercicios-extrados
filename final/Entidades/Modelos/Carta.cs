using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades.Modelos
{
    public class Carta
    {
        public int CartaID { get; set; }
        public string? Nombre { get; set; }
        public int Ataque { get; set; }
        public int Defensa { get; set; }
        public string? Ilustracion { get; set; }
        //Revisar tipo de dato

        // Relación muchos a muchos con Serie
        public ICollection<CartaSerie> CartaSeries { get; set; } = new List<CartaSerie>();
    }
}
