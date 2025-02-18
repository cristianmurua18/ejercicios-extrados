using Entidades.DTOs;
using Entidades.DTOs.Cruds;
using Entidades.DTOs.Jugadores;
using Entidades.DTOs.Respuestas;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Servicios.Servicios.Acceso;

namespace Juego.Controllers
{
    [Route("api/[controller]")]
    [AllowAnonymous] //esto hace que no sea necesaria una autorizacion, cualquiera puede usarlo
    [ApiController]
    public class AccesoController(IAccesoServicio accesoServicio) : ControllerBase
    {

        private readonly IAccesoServicio _accesoServicio = accesoServicio;


        //Iniciar sesion no es necesario
        [HttpGet]
        [Route("ObtenerPokemones")]
        public async Task<IActionResult> ObtenerPokemones()
        {

            var res = await _accesoServicio.ObtenerPokemones();

            return res ? Ok($"{res}") : BadRequest("No fue posible insertar");

        }

        [HttpGet]
        [Route("ObtenerCartaSerie")]
        public async Task<IActionResult> ObtenerCartaSerie()
        {

            var res = await _accesoServicio.RellenarCartaSerie();

            return res ? Ok($"{res}") : BadRequest("No fue posible insertar");

        }


        [HttpGet]
        [Route("ObtenerPais")]
        public async Task<IActionResult> ObtenerPais(string nombre)
        {
            var mensaje = string.Empty;

            var paises = await _accesoServicio.ObtenerIdPais(nombre);

            if(paises == null)
            {
                BadRequest("No existe pais con ese nombre. Pruebe nuevamente");
            }
            
            foreach (var pais in paises!)
            {
                mensaje += $"Nombre Pais: {pais.NombrePais}, IdPais: {pais.PaisID} \n";
            }
            return Ok(mensaje);

        }

        [HttpGet]
        [Route("ObtenerPaginaPais")]
        public async Task<IActionResult> ObtenerPaginacionPaises(int desdePagina, int cantRegistros)
        {
            var result = await _accesoServicio.ObtenerPaginacionPaises(desdePagina, cantRegistros);

            if (result.IsNullOrEmpty())
                return BadRequest("No fue posible obtener datos");

            return Ok(result);

        }



        //IDEA REVISAR EL ESTADO DE UN TORNEO Y QUE EN BASE A ESO TE DEJE INSCRIBIR O NO
        [HttpPost]
        [Route("RegistroJugador")]
        public async Task<IActionResult> RegistroJugador(CrudUsuarioDTO jugador)
        {
            if (await _accesoServicio.RegistroJugador(jugador))
            {
                return Ok($"Registro exitoso {jugador.Alias}. Bienvenido/a.");
            }
            return BadRequest("Registro fallido. Revise informacion");

            //FALTA ver como registrar los mazos de cartas que tiene al sistema
            //Inscripcion de mazo al torneo, ver que cumplan con las series de cartas permitidas
           //Cuando el organizador haga la inscripcion
        }


        [HttpPost]
        [Route("LoginUsuarios")]
        public async Task<IActionResult> Login(LoginDTO usuario)
        {
            return Ok(await _accesoServicio.ObtenerAutenticacion(usuario));

        }
    }
}
