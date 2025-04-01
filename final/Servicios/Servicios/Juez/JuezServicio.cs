using AccesoDatos.DAOs.Juez;
using Entidades.DTOs.Jugadores;
using Entidades.DTOs.Respuestas;
using Entidades.Modelos;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Servicios.Servicios.Juez
{
    public class JuezServicio(IDAOJuez daoJuez, IHttpContextAccessor httpContextAccessor) : IJuezServicio
    {
        private readonly IDAOJuez _daoJuez = daoJuez;

        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

        public async Task<string> VerRondasYPartidas(int idTorneo)
        {
            var idUsuario = Convert.ToInt32(_httpContextAccessor.HttpContext?.User.FindFirst("UsuarioID")?.Value);

            var corroborarPermiso = await _daoJuez.CorroborarJuezYTorneo(idTorneo, idUsuario);

            if (corroborarPermiso != null)
            {
                var res = await _daoJuez.VerRondasYPartidas(idTorneo);
                if (res.Count == 0)
                    return $"";
                else
                {
                    var mensaje = string.Empty;
                    foreach (var item in res)
                    {
                        mensaje += $"Numero Ronda: {item.NumeroRonda}, PartidaID: {item.PartidaID}, numero partida: {item.NumeroPartida}, fecha inicio: {item.FyHInicioP}, jugador 1: {item.JugadorUno}, jugador 2: {item.JugadorDos}, ganador: {item.Ganador}";
                    }
                    return mensaje;
                }
            }
            return $"No tienes permisos suficientes";
             
        }
        public async Task<bool> OficializarResultadoEnPartida(int idTorneo, int idPartida, int idGanador)
        {
            var idUsuario = Convert.ToInt32(_httpContextAccessor.HttpContext?.User.FindFirst("UsuarioID")?.Value); ;

            var corroborarPermiso = await _daoJuez.CorroborarJuezYTorneo(idTorneo, idUsuario);

            if(corroborarPermiso != null)
            { 

                var partidas = await _daoJuez.VerRondasYPartidas(idTorneo);

                if(partidas.Count == 0) throw new InvalidOperationException("No tal hay partida");

                foreach (var partida in partidas)
                {   //REVISAR SI ESTA BIEN ESTA VALIDACION
                    if (partida.JugadorUno != idGanador && partida.JugadorDos != idGanador)
                        throw new InvalidOperationException("El id no corresponde a ningun jugador de la partida");
                }

                return await _daoJuez.OficializarResultadoEnPartida(idTorneo, idPartida, idGanador);
            }
            return false;

        }

        public async Task<bool> OficializarResultadoEnTorneo(int idGanador, int idTorneo)
        {
            var idUsuario = Convert.ToInt32(_httpContextAccessor.HttpContext?.User.FindFirst("UsuarioID")?.Value); ;

            var corroborarPermiso = await _daoJuez.CorroborarJuezYTorneo(idTorneo, idUsuario);

            if (corroborarPermiso != null)
            {
                var torneo = await _daoJuez.VerTorneo(idTorneo);

                if (torneo != null)
                {
                    return await _daoJuez.OficializarResultadoEnTorneo(idGanador, idTorneo);
                }
               
            }
            return false;

        }


        public async Task<bool> DescalificarJugador(JugadorDescalificadoDTO jugadorDescalificado)
        {
            var idUsuario = Convert.ToInt32(_httpContextAccessor.HttpContext?.User.FindFirst("UsuarioID")?.Value); ;
            
            var corroborarPermiso = await _daoJuez.CorroborarJuezYTorneo(jugadorDescalificado.IdTorneo, idUsuario);

            if(corroborarPermiso != null)
            {
                return await _daoJuez.DescalificarJugador(jugadorDescalificado);
            }
            return false;
            
        }
    }
}
