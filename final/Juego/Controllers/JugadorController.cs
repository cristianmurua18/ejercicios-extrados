using Entidades.DTOs.Cruds;
using Entidades.Modelos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Servicios.Servicios.Jugador;

namespace Juego.Controllers
{
    [Route("api/[controller]")]
    [Authorize(Roles = "Jugador")]
    [ApiController]
    public class JugadorController(IJugadorServicio jugadorServicio) : ControllerBase
    {
        private readonly IJugadorServicio _jugadorServicio = jugadorServicio;

        #region Métodos exclusivos para Jugadores

        /// <summary>
        /// Aqui el jugador crea un mazo de cartas, con su correspondiente nombre y se le asigna un ID
        /// </summary>
        [HttpPost("CrearMazo")]
        public async Task<IActionResult> CrearMazo(string nombreMazo)
        {
            var res = await _jugadorServicio.CrearMazo(nombreMazo);
            if (res > 0)
                return Ok($"MazoID: {res}. Recuerdalo");
            return BadRequest("No fue posible crear el mazo.");

        }


        /// <summary>
        /// Aqui registrar las cartas con su Mazo anteriormente creado, se pide como referencia en que torneo lo va a usar
        /// para ver si la serie de las cartas que agrega las acepta o no
        /// </summary>
        [HttpPost("RegistroCartas")]
        public async Task<IActionResult> RegistrarCartas(CrudMazoCartasDTO cartas, int idTorneo)
        {
            return Ok(await _jugadorServicio.RegistrarCartas(cartas, idTorneo));

        }













        #endregion Fin Métodos de jugadores

    }
}
