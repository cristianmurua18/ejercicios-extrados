using APP.Entities.Models;
using APP.Services.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Ppal.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LibroController(ILibroService libroService) : ControllerBase
    {

        private readonly ILibroService _libroService = libroService;

        [HttpGet("obtenerlibros")]
        public IActionResult ObtenerLibros()
        {
            return Ok(_libroService.GetAllLibros());
        }

        [HttpPost("agregarlibros")]
        public IActionResult AgregarLibro(Libro libro)
        {
            return Ok(_libroService.InsertLibro(libro));
        }


        [HttpPost("obtenertoken")]
        [Authorize(Roles = "Usuario")]
        public IActionResult Login(int userID)
        {                         //recibir usuario y UserID, que debe coincidir con el prestamo

            try
            {
                ////Intento ubicar el usuario
                var users = _libroService.ObtenerUsuarios();
                ////revisar que el usuario es correcto
                var usuario = users.FirstOrDefault(us => us.Id == userID) ?? throw new Exception("Usuario no encontrado.");


                //Intento ubicar el prestamo - Id
                var prestamos = _libroService.Prestamos();

                //
                var result = prestamos.FirstOrDefault(prestamos => prestamos.UsuarioID == userID) ?? throw new Exception("Prestamo no otorgado");
                //revisar que el Id sea correcta y luego devolver el token

                if (result != null)
                {
                    return Ok(_libroService.GenerateJwt(usuario)); //Usuario autorizado, devuelvo el token
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
        [Authorize(Roles = "Usuario")]
        public IActionResult RetirarLibro(int libroId)
        {
            return Ok(_libroService.ModificarLibro(libroId)); //Cambia la propiedad disponible por false
        }


        //Post agrega un recurso al servidor
        [HttpPost("prestamo")]
        public IActionResult NewPrestamo(Prestamo prestamo)
        {

            return Ok(_libroService.AgregarPrestamo(prestamo));

        }

        [HttpGet("obtenerlistadoprestamos")]
        public IActionResult GetPrestamos()
        {
            return Ok(_libroService.Prestamos());
        }
    }
}
