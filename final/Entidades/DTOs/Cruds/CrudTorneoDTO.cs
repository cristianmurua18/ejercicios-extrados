using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades.DTOs.Cruds
{
    public class CrudTorneoDTO
    {
        public int TorneoID { get; set; }
        public DateTime FyHInicioT { get; set; }
        public DateTime FyHFinT { get; set; }
        public string? Estado { get; set; }
        public int? IdPaisRealizacion { get; set; }
        public int? PartidaActual { get; set; }
        public int? JugadorGanador { get; set; }
        public int? Organizador { get; set; }
        public int? PartidasDiarias { get; set; }
        public int? DiasDeDuracion { get; set; }
        public int? MaxJugadores { get;set; }

    }
}
