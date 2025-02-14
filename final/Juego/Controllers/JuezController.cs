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

        //ESTE METODO ESTA DE MAS
        //[Authorize(Roles = "Juez")]
        //[HttpPost("RegistrarJuez")]
        //public async Task<IActionResult> RegistroJuez([FromBody] CrudUsuarioDTO usuario)
        //{
        //    //Verificacion Inicial para que solo pueda crear Jueces. 
        //    if (usuario.Rol == "Juez")
        //    {
        //        //Validaciones basicas
        //        if (usuario == null) return BadRequest();
        //        //Si un modelo no es valido, valida estado del formulario, si alguna validacion no se cumple
        //        if (!ModelState.IsValid) return BadRequest(ModelState);
        //        //devolver un NoContent()?
        //        return Ok();
        //        //await _juezServicio.InsertarUsuario(usuario)
        //        //FUNCIONO

        //    }

        //    return BadRequest("No es posible insertar este tipo de usuario.");

        //}


        #endregion Fin exclusivos para Jueces
    }
}
