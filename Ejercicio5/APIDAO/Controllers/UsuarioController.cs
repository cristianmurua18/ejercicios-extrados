using Biblio.DAOs;
using Biblio.Models;
using Microsoft.AspNetCore.Mvc;

namespace APIDAO.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        //asigno instancia de DAOUsuario al UsuarioService, para que deje de ser null y podes usar sus metodos
        private static DAOUsuario? UsuarioService { get; set; } = DAOUsuario.Instance;


        [HttpGet("ObtenerUsuarios")]
        public IActionResult ObtenerUsuarios()
        {
            var result = UsuarioService.ObtenerListadoUsuarios();

            var lista = result.ToList();

            return Ok(lista);
        }


        //Forma de pedir un parametro para la peticion.
        [HttpGet("ObtenerUsuarioPorEmail/{email}")]
        public IActionResult ObtenerUsuarioPorEmail(string email)
        {
            var result = UsuarioService.ObtenerUsuarioPorEmail(email);

            //ver de implementar operador ternario. OK
            return Ok(result == null ? NotFound() : $"Id: {result.Id},Nombre: {result.Nombre}, Email:{result.Email}, Edad:{result.Edad}");
        
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



        [HttpPost("Login")]
        public IActionResult Login(Usuario usuario, string pass)
        {

            var result = UsuarioService.Login(usuario, pass);

            return Ok(result);

        }




    }
}
