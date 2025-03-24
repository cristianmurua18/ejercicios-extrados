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


        ///// <summary>
        ///// Sirve para rellenar la tabla de de Cartas
        ///// </summary>
        [HttpGet]
        [Route("ObtenerPokemones")]
        public async Task<IActionResult> ObtenerPokemones()
        {

            var res = await _accesoServicio.ObtenerPokemones();

            return res ? Ok($"{res}") : BadRequest("No fue posible insertar");

        }

        ///// <summary>
        ///// Sirve para rellenar la tabla de de CartasSerie
        ///// </summary>
        [HttpGet]
        [Route("ObtenerCartaSerie")]
        public async Task<IActionResult> ObtenerCartaSerie()
        {

            var res = await _accesoServicio.RellenarCartaSerie();

            return res ? Ok($"{res}") : BadRequest("No fue posible insertar");

        }

        //[HttpPost]
        //[Route("CambiarContasena")]
        //public async Task<IActionResult> CambiarContraseña(string nuevaContraseña, int userId)
        //{
        //    if (await _accesoServicio.CambiarContrasena(nuevaContraseña, userId) > 0)
        //    {
        //        return Ok($"Cambio de contraseña exitoso");
        //    }
        //    return BadRequest("Cambio de contraseña fallido. Revise informacion.");
        //}

        /// <summary>
        /// Sirve para tener Informacion de referencia de los torneos disponibles, el id sirve para inscribir un jugador
        /// </summary>
        [HttpGet]
        [Route("VerInfoTorneos")]
        public async Task<IActionResult> VerInfoTorneos()
        {
            return Ok(await _accesoServicio.VerInfoTorneos());
        }

        /// <summary>
        /// Sirve para tener Informacion de referencia de los paises disponibles por Nombre, te da una lista
        /// </summary>
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

        /// <summary>
        /// Sirve para tener Informacion de referencia de los paises disponibles por cierta cantidad
        /// </summary>
        [HttpGet]
        [Route("ObtenerPaginaPais")]
        public async Task<IActionResult> ObtenerPaginacionPaises(int desdePagina, int cantRegistros)
        {
            var result = await _accesoServicio.ObtenerPaginacionPaises(desdePagina, cantRegistros);

            if (result.IsNullOrEmpty())
                return BadRequest("No fue posible obtener datos");

            return Ok(result);
            

        }

        /// <summary>
        /// Sirve para que un jugador pueda inscribirse como usuario
        /// Se necesita la referencia de un torneoID con interes de inscripcion, se revisa su estado
        /// </summary>
        [HttpPost]
        [Route("RegistroJugador")]
        public async Task<IActionResult> RegistroJugador(CrudUsuarioDTO jugador)
        {
            if (await _accesoServicio.RegistroJugador(jugador))
            {
                return Ok($"Registro exitoso {jugador.Alias}. Bienvenido/a. Recuerde usuario y contraseña.");
            }
            return BadRequest("Registro fallido. Revise informacion.");

            //FALTA ver como registrar los mazos de cartas que tiene al sistema
            //Inscripcion de mazo al torneo, ver que cumplan con las series de cartas permitidas
           //Cuando el organizador haga la inscripcion
        }

        /// <summary>
        /// Sirve para que un usuario pueda obtener el token de autorizacion
        /// </summary>
        [HttpPost]
        [Route("LoginUsuarios")]
        public async Task<IActionResult> Login(LoginDTO usuario)
        {
            return Ok(await _accesoServicio.ObtenerAutenticacion(usuario));

        }
    }
}
