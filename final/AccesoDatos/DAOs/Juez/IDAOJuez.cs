using Entidades.DTOs;
using Entidades.DTOs.Jugadores;
using Entidades.DTOs.Respuestas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccesoDatos.DAOs.Juez
{
    public interface IDAOJuez
    {
        public Task<TorneoDTO> VerTorneo(int idTorneo);
        public Task<TorneoJuecesDTO> CorroborarJuezYTorneo(int idTorneo, int idUsuario);
        public Task<List<PartidaRondaDTO>> VerRondasYPartidas(int idTorneo);
        public Task<bool> OficializarResultadoEnPartida(int idTorneo, int idPartida, int idGanador);
        public Task<bool> OficializarResultadoEnTorneo(int idGanador, int idTorneo);
        public Task<bool> DescalificarJugador(JugadorDescalificadoDTO jugadorDescalificado);

    }

}
