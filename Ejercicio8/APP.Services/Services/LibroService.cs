using APP.Entities.Daos;
using APP.Entities.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace APP.Services.Services
{
    public class LibroService(IDAOLibro daoLibro, IConfiguration jwtconfig) : ILibroService
    {

        private readonly IDAOLibro _DaoLibro = daoLibro;

        private readonly IConfiguration _configJwt = jwtconfig;

        public List<Libro> GetAllLibros()
        {
            return _DaoLibro.GetLibroList();

        }

        public string InsertLibro(Libro libro)
        {
            _DaoLibro.InsertLibro(libro);
            return "Libro insertado";

        }

        public string AgregarPrestamo(Prestamo prestamo)
        {
            _DaoLibro.NuevoPrestamo(prestamo);

            return $"Recuerde la fecha de devolucion: {prestamo.FechaDevolucion}";

        }

        public List<Prestamo> Prestamos()
        {
            return _DaoLibro.GetListPrestamos();
            
        }


        public List<Usuario> ObtenerUsuarios()
        {
            return _DaoLibro.GetUsuarios();

        }


        //Debe recibir el usuario Autenticado
        public string GenerateJwt(Usuario user)
        {

            //La llave se puede recibir del appsettings.json como una key - IMPORTANTE REVISAR COMO HACERLO y configurar
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configJwt["Jwt:Key"]));

            //Aqui indico que tipo de algoritmo voy a utilizar
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);


            //Creo los Claims o reclamaciones - Se devuelven como parte del token en Paylood
            var claims = new List<Claim>
            {   //Reclamaciones
                new(ClaimTypes.NameIdentifier, user.UserName),
                new(ClaimTypes.Email, user.EmailAddress),
                new(ClaimTypes.GivenName, user.FirstName),
                new(ClaimTypes.Surname, user.LastName),
                //new Claim("Id", user.Id),  //Aca deberia ir el Id del usuario obtenido de la BD
                new(ClaimTypes.Role, user.Rol)  //Aca deberia ir el rol del usario obtenido de la BD
            };

            //Creo el Token
            var Sectoken = new JwtSecurityToken(_configJwt["Jwt:Issuer"], //Issuer
                _configJwt["Jwt:Audience"], //Audiencia
                claims: claims,  //Reclamaciones
                expires: DateTime.Now.AddMinutes(120),  //Tiempo de expiracion del token
                signingCredentials: credentials  //Credenciales
                );

            //Escribe el token en texto plano
            var token = new JwtSecurityTokenHandler().WriteToken(Sectoken);

            return token;
        }

        public string ModificarLibro(int id)
        {
            return _DaoLibro.ModificarLibro(id);
        }


        public string GenerarRefreshToken()
        {
            var byteArray = new byte[64];

            var refreshToken = "";

            using (var rng = RandomNumberGenerator.Create()) 
            {
                rng.GetBytes(byteArray);

                refreshToken = Convert.ToBase64String(byteArray);
            }
            return refreshToken;
        }
       
        

    }
}
