using APP.Entities.Daos;
using APP.Entities.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace APP.Services.Services
{
    public class UsuarioService(IDAOUsuario daoUser, IConfiguration jwtconfig) : IUsuarioService
    {

        private readonly IDAOUsuario _DaoUser = daoUser;

        private readonly IConfiguration _configJwt = jwtconfig;


        //Implementacion de los metodos que luego se usan en los controladores

        public List<Usuario> GetAll()
        {
            var lista = _DaoUser.ObtenerListadoUsuarios();

            return lista;
        }

        public Usuario GetByEmailAddress(string email)
        {
            var user = _DaoUser.ObtenerUsuarioPorEmail(email);

            return user;
        }

        //Ver de hacerlos - a los 3 siguientes - void
        public string AddUser(Usuario usuario)
        {
            _DaoUser.AgregarUsuario(usuario);

            return "Usuario agregado.";

        }

        public string UpdateUserByID(Usuario usuario)
        {
            _DaoUser.ModificarUsuarioPorID(usuario);

            return "Usuario Modificado.";
        }


        public string DeleteUserByID(Usuario usuario)
        {
            _DaoUser.BorrarUsuarioPorID(usuario);

            return "Usuario borrado.";
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
                expires: DateTime.Now.AddMinutes(120),  //Tiempo de expiracion del token, puedo utilizar utcNow
                signingCredentials: credentials  //Credenciales
                );

            //Escribe el token en texto plano
            var token = new JwtSecurityTokenHandler().WriteToken(Sectoken);

            return token;
        }

        /*
        Ver de implementar un metodo de Authenticate(UserLogin usuarioLogin)? y comparar con los datos del usuario. Min 15:50 aprox
        Si pasa la autenticacion, entonces devuelver el token
        */
        public bool Authenticate(Usuario loginUser, string password)
        {
            //Primero hacer el Hash de la contraseña de LoginUser, que recibi por parametro
            //NO ES EL MISMO HASH, no funciona

            var verif = VerifyPassword(loginUser, password);

            return verif == true && verif;

        }

        public bool VerifyPassword(Usuario usuario, string password)
        {
            var pass = new PasswordHasher<Usuario>();

            var result = pass.VerifyHashedPassword(usuario, usuario.PasswordHash, password);

            return result == PasswordVerificationResult.Success;
        }

    }
}
