using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades.Modelos
{
    public class CartaSerie
    {
        public int CartaID { get; set; }
        public Carta? Carta { get; set; }
        public int SerieID { get; set; }
        public Serie? Serie { get; set; }



    }
}
