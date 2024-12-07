using APP.Entities.Models;
using APP.Service.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
//using System.Web.Http;

namespace APIDAO.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {

        //Propiedad para la Inyeccion de dependencia de la Capa Servicios
        private readonly IUserService _userService;

        //En el constructor de la clase inyecto el servicio y lo guardo en la propiedad
        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        //VER de limitar el alcance solo a usuarios logeados y con Rol establecido
        [Authorize(Roles = "Administrador")]
        [HttpGet("obtenerusuarios")]
        public IActionResult ObtenerUsuarios()
        {
            var result = _userService.GetAll();

            var lista = result.ToList();

            return Ok(lista);
        }


        //Forma de pedir un parametro para la peticion? /{email}?
        [Authorize(Roles = "Administrador")]
        [HttpGet("obtenerusuarioporemail")]
        public IActionResult ObtenerUsuarioPorEmail(string email)
        {
            var result = _userService.GetByEmailAddress(email);

            //Capaz pueda mandar un BadRequest() si no se encuentra el email - REVISAR
            return result == null ? NotFound("Usuario no encontrado") : Ok(result);

        }

        [Authorize(Roles = "Administrador")]
        [HttpPost("AgregarUsuario")]
        public IActionResult AgregarUsuario(UserModel usuario)
        {

            var usr = _userService.AddUser(usuario);

            return Ok(usr);

        }

        [Authorize(Roles = "Administrador")]
        [HttpPut("ActualizarUsuarioPorID")]
        public IActionResult ActualizarUsuarioPorID(UserModel usuario)
        {
            var result = _userService.UpdateUserByID(usuario);

            return Ok(result);
        }

        [Authorize(Roles = "Administrador")]
        [HttpDelete("BorrarUsuarioPorID")]
        public IActionResult BorrarUsuarioPorID(UserModel usuario)
        {
            var result = _userService.DeleteUserByID(usuario);

            return Ok(result);
        }


        //Paso 3. Hacer un endpoint de login, para poder generar token de autorizacion y generar el jwt
        [HttpPost("Login")]
        public IActionResult Login(string user, string password)
        {                         //recibir usuario y contraseña por parametro o body. OK

            try
            {
                //Intento ubicar el usuario
                var users = _userService.GetAll();
                //revisar que el usuario es correcto
                var actual = users.FirstOrDefault(us => us.UserName == user) ?? throw new Exception("Usuario no encontrado.");


                //Para que necesito recibir un UserModel? Por el metodo autenticacion
                var result = _userService.Authenticate(actual, password);
                //revisar que la contra sea correcta y luego devolver la informacion del usuario

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

        //Authorize(Roles = ("Administrador"))
        [Authorize(Roles = "Administrador")]
        [HttpGet("ObtenerInformacionSecreta")]
        public string GetUsuario()
        {
            return "OIS!!!";
        }




    }
}
