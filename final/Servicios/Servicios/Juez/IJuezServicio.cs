using Entidades.DTOs.Jugadores;
using Entidades.DTOs.Respuestas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Servicios.Servicios.Juez
{
    public interface IJuezServicio
    {
        public Task<string> VerRondasYPartidas(int idTorneo);
        public Task<bool> OficializarResultadoEnPartida(int idTorneo, int idPartida, int idGanador);
        public Task<bool> OficializarResultadoEnTorneo(int idGanador, int idTorneo);
        public Task<bool> DescalificarJugador(JugadorDescalificadoDTO jugadorDescalificado);

    }
}
