using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades.Modelos
{
    public class Torneo
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


        //Une con Partida
        public int PartidaID { get; set; }
        public DateTime FyHInicioP { get; set; }
        public DateTime FyHFinP { get; set; }
        public string? Ronda { get; set; }
        public int JugadorDerrotado { get; set; }
        public int JugadorVencedor { get; set; }

    }
}


//RESPUESTA. ES UNA LISTA
//[
//  {
//    "torneoID": 1,
//    "nombreTorneo": "Devastacion",
//    "fyHInicioT": "2025-03-28T10:00:00",
//    "fyHFinT": "0001-01-01T00:00:00",
//    "estado": "Finalizado",
//    "idPaisRealizacion": 0,
//    "jugadorGanador": 0,
//    "organizador": 0,
//    "partidasDiarias": 0,
//    "diasDeDuracion": 0,
//    "maxJugadores": 0
//  },
//  {
//    "torneoID": 3,
//    "nombreTorneo": "Armagedon",
//    "fyHInicioT": "2025-04-02T10:00:00",
//    "fyHFinT": "0001-01-01T00:00:00",
//    "estado": "Partidas",
//    "idPaisRealizacion": 0,
//    "jugadorGanador": 0,
//    "organizador": 0,
//    "partidasDiarias": 0,
//    "diasDeDuracion": 0,
//    "maxJugadores": 0
//  }
//]