using Entidades.DTOs.Jugadores;
using Entidades.DTOs.Respuestas;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Servicios.Servicios.Juez;

namespace Juego.Controllers
{
    [Route("api/[controller]")]
    [Authorize(Roles = "Juez")]
    [ApiController]
    public class JuezController(IJuezServicio juezServicio) : ControllerBase
    {
        private readonly IJuezServicio _juezServicio = juezServicio;

        #region Métodos exclusivos para Jueces

        /// <summary>
        /// Pueden oficializar el resultado de un juego dentro de un evento, y descalificar un jugador de ser necesario.
        /// Los debe crear un Administrador o un Juez
        /// </summary>

        [HttpGet("ObtenerPartidasYRondas")]
        public async Task<IActionResult> VerRondasYPartidas(int idTorneo)
        {
            var res = await _juezServicio.VerRondasYPartidas(idTorneo);
            if (res.IsNullOrEmpty())
            {
                return NotFound("No hay rondas ni partidas generadas por el momento");
            }
            return Ok(res);
        }

        [HttpPost("OficializarPartida")]
        public async Task<IActionResult> OficializarResultadoEnPartida(int idTorneo, int idPartida, int idGanador)
        {
            if (await _juezServicio.OficializarResultadoEnPartida(idTorneo, idPartida, idGanador))
                return Ok($"Se inserto al ganador de la Partida: N° {idPartida}");
            return BadRequest("No fue posible insertar al ganador");
        }

        [HttpPost("OficializarTorneo")]
        public async Task<IActionResult> OficializarResultadoEnTorneo(int idTorneo, int idGanador)
        {
            if (await _juezServicio.OficializarResultadoEnTorneo(idTorneo, idGanador))
                return Ok($"Se inserto al ganador del Torneo: N° {idTorneo}");
            return BadRequest("No fue posible insertar al ganador");
        }



        [HttpPost("DescalificarJugador")]
        public async Task<IActionResult> DescalificarJugador(JugadorDescalificadoDTO jugadorDescalificado)
        {
            if (await _juezServicio.DescalificarJugador(jugadorDescalificado))
                return Ok($"Jugador {jugadorDescalificado.JugadorID} descalificado con exito del torneo {jugadorDescalificado.IdTorneo}.");
            return BadRequest("No fue posible descalificar. Revise informacion");
        }


        #endregion Fin exclusivos para Jueces
    }
}
