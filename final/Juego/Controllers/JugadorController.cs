using Entidades.DTOs.Cruds;
using Entidades.Modelos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
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
        /// Aqui el jugador crea una coleccion de cartas, con su correspondiente id de la carta
        /// </summary>
        [HttpPost("AgregarCartaAMiColeccion")]
        public async Task<IActionResult> CrearColeccion(int idCarta)
        {
            var res = await _jugadorServicio.CrearColeccion(idCarta);
            if (res > 0)
                return Ok($"Carta agregada con exito");
            return BadRequest("No fue posible agregar la carta");

        }

        /// <summary>
        /// Aqui el jugador crea un mazo de cartas, con su correspondiente nombre y se le asigna un ID
        /// </summary>
        [HttpPost("CrearMazo")]
        public async Task<IActionResult> CrearMazo(string nombreMazo)
        {
            var res = await _jugadorServicio.CrearMazo(nombreMazo);
            if (res > 0)
                return Ok($"Mazo creado con exito. Su ID es: {res}. Recuerdalo para poder registrar cartas");
            return BadRequest("No fue posible crear el mazo.");

        }

        /// <summary>
        /// Aqui el jugador puede ver sus mazos creados
        /// </summary>
        [HttpGet("VerMisMazos")]
        public async Task<IActionResult> VerMisMazos()
        {
            var mazos = await _jugadorServicio.VerMisMazos();

            if (!mazos.IsNullOrEmpty())
            {
                return Ok(mazos);
            }
            return BadRequest("Ud. no tiene un mazo creado.");
            //Que pasa si tiene varios Mazos creados?
        }




        /// <summary>
        /// Aqui registrar las cartas con su Mazo anteriormente creado, se pide como referencia en que torneo lo va a usar
        /// para ver si la serie de las cartas que agrega las acepta o no
        /// </summary>
        [HttpPost("RegistroCartasAMiMazo")]
        public async Task<IActionResult> RegistrarCartas(CrudMazoCartasDTO cartas, int idTorneo)
        {
            return Ok(await _jugadorServicio.RegistrarCartas(cartas, idTorneo));

        }

        /// <summary>
        /// Aqui se puede registrar a un torneo, pasar idTorneo y idMazo
        /// </summary>
        [HttpPost("RegistroEnTorneo")]
        public async Task<IActionResult> RegistroEnTorneo(int idTorneo, int idMazo)
        {
            if (await _jugadorServicio.RegistroEnTorneo(idTorneo, idMazo))
                return Ok("Registro exitoso. Bienvenido!");
            return BadRequest("Registro fallido. Revise informacion");

        }


        //Cual seria mejor opcion? Que se incriba solo o que inscriba el mazo el organizador?










        #endregion Fin Métodos de jugadores

    }
}
