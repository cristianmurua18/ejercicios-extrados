using Biblio.DAOs;
using Biblio.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace APIDAO.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private static DAOUsuario? UsuarioService { get; set; }

        public UsuarioController()
        { //asigno instancia de DAOUsuario al UsuarioService, para que deje de ser null
            UsuarioService = DAOUsuario.Instance;
        }

        [HttpGet("ObtenerUsuarios")]
        public IActionResult ObtenerUsuarios()
        {
            var result = UsuarioService.ObtenerListadoUsuarios();

            var lista = result.ToList();

            return Ok(lista);
        }

        ///{email}
        //Forma de pedir un parametro para la peticion
        [HttpGet("ObtenerUsuarioPorEmail")]
        public IActionResult ObtenerUsuarioPorEmail(string email)
        {
            var result = UsuarioService.ObtenerUsuarioPorEmail(email);

            //ver de implementar operador ternario
            return Ok(result == null ? $"No encontrado." : $"Id: {result.Id}, Nombre: {result.Nombre}, Email: {result.Email}, Edad: {result.Edad}");
        
        }

        [HttpPost("AgregarUsuario")]
        public IActionResult AgregarUsuario(Usuario usuario)
        {

            var usr = UsuarioService.AgregarUsuario(usuario);

            return Ok(usr);
            
        }

        [HttpPut("ActualizarUsuarioPorID")]
        public IActionResult ActualizarUsuarioPorID(Usuario usuario)
        {
            var result = UsuarioService.ModificarUsuarioPorID(usuario);

            return Ok(result);
        }

        [HttpDelete("BorrarUsuarioPorID")]
        public IActionResult BorrarUsuarioPorID(Usuario usuario)
        {
            var result = UsuarioService.BorrarUsuarioPorID(usuario);

            return Ok(result);
        }






    }
}
