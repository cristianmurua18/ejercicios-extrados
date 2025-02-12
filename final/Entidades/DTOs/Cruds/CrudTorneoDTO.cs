using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades.DTOs.Cruds
{
    public class CrudTorneoDTO
    {
        public DateTime FyHInicio { get; set; }
        public DateTime FyHFin { get; set; }
        public string? Pais { get; set; }
        public string? Fase { get; set; }
        public int RondaActual { get; set; }
        public int JugadorGanador { get; set; }
        public int Organizador { get; set; }

    }
}
