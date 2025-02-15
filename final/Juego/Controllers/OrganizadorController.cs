using Entidades.DTOs.Cruds;
using Entidades.Modelos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Servicios.Servicios.Organizador;

namespace Juego.Controllers
{
    [Route("api/[controller]")]
    [Authorize(Roles = "Organizador")]
    [ApiController]
    public class OrganizadorController(IOrganizadorServicio organizadorServicio) : ControllerBase
    {

        private readonly IOrganizadorServicio _organizadorServicio = organizadorServicio;


        #region Métodos exclusivos para Organizadores
        /// <summary>
        /// Pueden crear, editar y cancelar torneos
        /// Pueden hacer avanzar un torneo a la siguiente fase, y modificar las partidas del torneo
        /// Los debe crear un Administrador, pueden crear Jueces
        /// </summary>
        /// 

        //[HttpGet("ObtenerTorneos")]
        //public async Task<IActionResult> ObtenerTorneos()
        //{
        //    //return StatusCode(StatusCodes.Status200OK, new { Value = _usuarioServicio.ObtenerUsuarios() });
        //    return Ok(await _organizadorServicio.VerTorneos());
        //    //FUNCIONA, ver paginacion. Ver que sean solo sus torneos organizados
        //}
        [HttpGet("ObtenerUsuariosPorRol")]
        public async Task<IActionResult> VerListadoJugadores(string rol)
        {
            if (rol != "Jugador" || rol != "Juez")
                return BadRequest("Solo puedes ver Jugadores o jueces creados por ti");

            return Ok(await _organizadorServicio.VerListadoUsuarios(rol));

        }


        [HttpPost("RegistroJuez")]
        public async Task<IActionResult> RegistrarJuez(CrudUsuarioDTO usuario)
        {
            //Verificacion Inicial para que solo pueda crear Jueces, en que sea su torneo organizado?. 
            if (usuario.Rol == "Juez")
            {
                //Validaciones basicas
                if (usuario == null) return BadRequest();
                //Si un modelo no es valido, valida estado del formulario, si alguna validacion no se cumple
                if (!ModelState.IsValid) return BadRequest(ModelState);
                //devolver un NoContent()?
                return Ok(await _organizadorServicio.RegistrarJuez(usuario));
                //FUNCIONO
            }

            return BadRequest("No es posible insertar otro tipo de usuario");

        }


        [HttpPost("OrganizarTorneo")]
        public async Task<IActionResult> OrganizarTorneo(CrudTorneoDTO torneo)
        {
            //Ver que sean solo sus torneos organizados
            return Ok(await _organizadorServicio.CrearTorneo(torneo));
        }

        [HttpPut("EditarTorneo")]
        public async Task<IActionResult> EditarTorneo(CrudTorneoDTO torneo)
        {
            //Ver que sean solo sus torneos organizados
            //Sirve para hacer avanzar a la siguiente fase un torneo
            return Ok(await _organizadorServicio.EditarTorneo(torneo));
        }

        [HttpPut("CancelarTorneo")]
        public async Task<IActionResult> CancelarTorneo(int torneoid, string texto)
        {
            //Ver que sean solo sus torneos organizados
            return Ok(await _organizadorServicio.CancelarTorneo(torneoid, texto));

        }



        //[HttpPost("CrearPartida")]
        //public async Task<IActionResult> CrearPartida(PartidaDTO partida)
        //{
        //    return Ok(await _usuarioServicio.CrearPartida(partida));
        //}


        ////A lo mejor falta un metodo para crear, modificar partidas
        //[HttpPost("ModificarPartida")]
        //public async Task<IActionResult> ModificarPartida(PartidaDTO partida)
        //{
        //    //modificar partidas solo creadas por el
        //    return Ok(await _usuarioServicio.ModificarPartida(partida));
        //}



        #endregion Final Organizadores
    }
}
