using APP.Entities.Models;
using APP.Services.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Ppal.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LibroController(ILibroService libroService) : ControllerBase
    {

        private readonly ILibroService _libroService = libroService;

        //Se obtiene el listado de libros disponibles, todo pueden obtenerlo
        [HttpGet("obtenerlibros")]
        public IActionResult ObtenerLibros()
        {
            return Ok(_libroService.GetAllLibros());

        }

        //Agregar nuevo libro. Posiblemente solo permitir al bibliotecario
        [Authorize(Roles = "Bibliotecario")]
        [HttpPost("agregarlibros")]
        public IActionResult AgregarLibro(Libro libro)
        {
            return Ok(_libroService.InsertLibro(libro));
        }

        //Obtener token para retirar libro, solo los usuarios puede retirar libros
        [HttpPost("obtenertoken")]
        //[Authorize(Roles = "usuario")]
        public IActionResult Autenticar(int userID)
        {                         //recibir UserID, que debe coincidir con el Usuario.Id y con el prestamo.UserID 

            try
            {

                //Intento ubicar el usuario, por su Id
                var users = _libroService.ObtenerUsuarios();
                //revisar que el usuario es correcto
                var usuario = users.FirstOrDefault(us => us.Id == userID) ?? throw new Exception("Usuario no encontrado.");


                //Intento ubicar el prestamo - por Id del usuario
                var prestamos = _libroService.Prestamos();

                //
                var result = prestamos.FirstOrDefault(prestamos => prestamos.UsuarioID == userID) ?? throw new Exception("Prestamo no otorgado");
                //revisar que el Id sea correcta y luego devolver el token

                if (result != null)
                {
                    var (Token, Claims) = _libroService.GenerateJwt(usuario);
                    //Intento ubicar el usuario, por su Id en los claims
                    var claim = Claims.FirstOrDefault(c => c.Type == "UsuarioID");

                    if (claim != null && int.Parse(claim.Value) == usuario.Id)
                    {
                        return Ok(Token); //Usuario autorizado, devuelvo el token
                    }
                    return BadRequest();
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

        //Es PUT para modificar informacion
        [HttpPut("retirarlibro")]
        [Authorize(Roles = "usuario")]
        public IActionResult RetirarLibro(int libroId)
        {
            return Ok(_libroService.ModifLibro(libroId)); //Cambia la propiedad disponible del libro por false
        }


        //Post agrega un recurso al servidor
        //Metodo para agregar un nuevo prestamo, posiblemente deba hacerlo el bibliotecario
        //Se debe informar al usuario el Id del prestamo y recordar Id del usuario
        [Authorize(Roles = "Bibliotecario")]
        [HttpPost("prestamo")]
        public IActionResult PedirLibro(Prestamo prestamo)
        {

            return Ok(_libroService.AgregarPrestamo(prestamo));

        }

        [Authorize(Roles = "Bibliotecario")]
        [HttpGet("obtenerlistadoprestamos")]
        public IActionResult GetPrestamos()
        {
            return Ok(_libroService.Prestamos());
        }



        [HttpGet]
        [Route("cookie")]
        public IActionResult Cookies()
        {
            //Orden de crear una cookie, en la respuesta del servicio 
            Response.Cookies.Append("Nombre", "DATO A GUARDAR", new CookieOptions()
            {
                Secure = true,  //En true solo se va a enviar a un servidor por HTTPS
                Expires = DateTime.UtcNow.AddMinutes(60), //En cuanto tiempo debe ser eliminada del cliente
                HttpOnly = true, //Asi no es accesible por el front (javascript)
                SameSite = SameSiteMode.None //Solo se envia al mismo servicio o pagina que la creo
            });

            //Forma de pedir una cookie
            //var cookie = Request.Cookies["Nombre"];

            //Se puede sobreescribir una cokie para borrarla en el momento con Expires = Datetime.Now;

            return Ok();

        }



    }
}
