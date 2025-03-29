using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades.DTOs.Respuestas
{
    public class PartidaRondaDTO
    {
        public int PartidaID { get; set; }
        public int IdRonda { get; set; }
        public int NumeroPartida { get; set; }
        public DateTime FyHInicioP { get; set; }
        public DateTime FyHFinP { get; set; }
        public int JugadorUno { get; set; }
        public int JugadorDos { get; set; }
        public int Ganador { get; set; }
        public int? RondaID { get; set; }
        public int? IdTorneo { get; set; }
        public int? NumeroRonda { get; set; }

    }
}
