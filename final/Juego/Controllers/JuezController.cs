using Entidades.DTOs.Jugadores;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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

        [HttpPost("OficializarResultados")]
        public async Task<IActionResult> OficializarResultados()
        {
            //Que debe hacer oficializar resultados?
            //Jugador ganador
            return Ok(await _juezServicio.OficializarResultados());
        }

        [HttpPost("DescalificarJugador")]
        public async Task<IActionResult> DescalificarJugador(JugadorDescalificadoDTO jugadorDescalificado)
        {
            return Ok(await _juezServicio.DescalificarJugador(jugadorDescalificado));
        }


        #endregion Fin exclusivos para Jueces
    }
}
