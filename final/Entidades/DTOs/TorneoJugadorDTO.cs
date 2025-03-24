using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades.DTOs
{
    public class TorneoJugadorDTO
    {
        public int IdTorneo { get; set; }
        public int IdJugador { get; set; }
        public int IdMazo { get; set; }
        public int Aceptada { get; set; }

    }
}
