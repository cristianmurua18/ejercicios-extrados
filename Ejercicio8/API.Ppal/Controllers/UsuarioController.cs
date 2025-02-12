using APP.Entities.Models;
using APP.Services.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Ppal.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController(IUsuarioService userService) : ControllerBase
    {
        //Propiedad para la Inyeccion de dependencia de la Capa Servicios
        private readonly IUsuarioService _userService = userService;

        //VER de limitar el alcance solo a usuarios logeados -Primero debo obtener el token- y con Rol establecido
        [Authorize(Roles = "Administrador")]
        [HttpGet("obtenerusuarios")]
        public IActionResult ObtenerUsuarios()
        {
            return Ok(_userService.GetAll());
        }


        //Forma de pedir un parametro para la peticion? /{email}?
        [Authorize(Roles = "Administrador")]
        [HttpGet("obtenerusuarioporemail")]
        public IActionResult ObtenerUsuarioPorEmail(string email)
        {
            var user = _userService.GetByEmailAddress(email);

            //Capaz pueda mandar un BadRequest() si no se encuentra el email - REVISAR
            return user == null ? NotFound("Usuario no encontrado") : Ok(user);

        }

        //[Authorize(Roles = "Administrador")]
        [HttpPost("AgregarUsuario")]
        public IActionResult AgregarUsuario(Usuario usuario)
        {
            _userService.AddUser(usuario);
            return Ok();

        }

        [Authorize(Roles = "Administrador")]
        [HttpPut("ActualizarUsuarioPorID")]
        public IActionResult ActualizarUsuarioPorID(Usuario usuario)
        {
            var result = _userService.UpdateUserByID(usuario);

            return Ok(result);
        }

        [Authorize(Roles = "Administrador")]
        [HttpDelete("BorrarUsuarioPorID")]
        public IActionResult BorrarUsuarioPorID(Usuario usuario)
        {
            var result = _userService.DeleteUserByID(usuario);

            return Ok(result);
        }


        //Paso 3. Hacer un endpoint de login, para poder generar token de autorizacion y generar el jwt
        //Hacer login solo de usuario?
        [HttpPost("Login")]
        public IActionResult Login(string user, string password)
        {                         //recibir usuario y contraseña por parametro o body. OK

            try
            {
                //Intento ubicar el usuario
                var users = _userService.GetAll();
                //revisar que el usuario es correcto
                var actual = users.FirstOrDefault(us => us.UserName == user) ?? throw new Exception("Usuario no encontrado.");


                //Para que necesito recibir un Usuario? Por el metodo autenticacion
                var result = _userService.Authenticate(actual, password);
                //revisar que la contra sea correcta para recien poder devolver el token

                if (result)
                {
                    return Ok(_userService.GenerateJwt(actual)); //Usuario autorizado, devuelvo el token
                }
                else
                {

                    return NotFound("Usuario no autorizado");  //Usuario no autorizado, NO Devuelvo el token
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        //Marca del metodo para que solo se puede mostrar si pasa la autorizacion con el token,
        //para mayor seguridad se puede agregar que un determinado rol solo tenga permiso para acceder

        
        [Authorize(Roles = "Administrador")]
        [HttpGet("ObtenerInformacionSecreta")]
        public string GetUsuario()
        {
            return "OIS!!!";
        }


        


    }
}
