using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades.DTOs
{
    public class RondaDTO
    {
        public int? RondaID { get; set; }
        public int? IdTorneo { get; set; }
        public int? CantidadPartidas { get; set; }
        public bool? JugadoresPar { get; set; }

    }
}
