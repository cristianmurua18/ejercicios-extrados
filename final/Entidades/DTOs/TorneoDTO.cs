﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades.DTOs
{
    public class TorneoDTO
    {
        public int TorneoID { get; set; }
        public string? NombreTorneo { get; set; }
        public DateTime FyHInicioT { get; set; }
        public DateTime FyHFinT { get; set; }
        public string? Estado { get; set; }
        public int IdPaisRealizacion { get; set; }
        public int JugadorGanador { get; set; }
        public int Organizador { get; set; }
        public int PartidasDiarias { get; set; }
        public int DiasDeDuracion { get; set; }
        public int MaxJugadores { get; set; }

        //Une con tabla Partidas
        //public int PartidaID { get; set; }
        //public DateTime FyHInicioP { get; set; }
        //public DateTime FyHFinP { get; set; }
        //public string? Ronda { get; set; }
        //public int JugadorDerrotado { get; set; }
        //public int JugadorVencedor { get; set; }

    }
}
