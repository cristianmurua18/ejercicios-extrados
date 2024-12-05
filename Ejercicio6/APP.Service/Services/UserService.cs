using APP.Entities.DAOs;
using APP.Entities.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace APP.Service.Services
{
    public class UserService : IUserService
    {
        //Aqui se debe consumir el DAO

        private readonly IDAOUser _DaoUser;


        private readonly IConfiguration _configJwt;

        public UserService(IDAOUser daoUser, IConfiguration jwtconfig)
        {
            _DaoUser = daoUser;

            _configJwt = jwtconfig;
        }
        

        //Implementacion de los metodos que luego se usan en los controladores

        public List<UserModel> GetLogin()
        {
            var lista = _DaoUser.ObtenerUsuariosParaLogin();

            return lista;
        }

        public List<UserModel> GetAll()
        {
            var lista = _DaoUser.ObtenerListadoUsuarios();

            return lista;
        }

        public UserModel GetByEmailAddress(string email)
        {
            var user = _DaoUser.ObtenerUsuarioPorEmail(email);

            return user;
        }

        public string AddUser(UserModel usuario)
        {
            var newUser = _DaoUser.AgregarUsuario(usuario);

            return "Usuario agregado.";

        }


        public string UpdateUserByID(UserModel usuario)
        {
            var update = _DaoUser.ModificarUsuarioPorID(usuario);

            return "";
        }


        public string DeleteUserByID(UserModel usuario)
        {
            var delete = _DaoUser.BorrarUsuarioPorID(usuario);

            return "";
        }

        //Debe recibir el usuario Autenticado
        public string GenerateJwt(UserModel user)
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

        /*
        Ver de implementar un metodo de Authenticate(UserLogin usuarioLogin)? y comparar con los datos del usuario. Min 15:50 aprox
        Si pasa la autenticacion, entonces devuelver el token
        */
        public bool Authenticate(UserModel loginUser, string password)
        {
            //Primero hacer el Hash de la contraseña de LoginUser, que recibi por parametro
            //loginUser.PasswordHash = _DaoUser.HashPassword(loginUser);
            //NO ES EL MISMO HASH, no funciona

            //Ver de acceder a User en la BD y comparar con loginUser
            //var users = _DaoUser.ObtenerUsuariosParaLogin();

            var verif = VerifyPassword(loginUser, password);

            return verif == true? verif : false;
            //var currentUser = users.FirstOrDefault(user => user.UserName == loginUser.UserName
            //&& user.PasswordHash == loginUser.PasswordHash);

            
        }

        //A lo mejor se puede mover al Servicio, para recien alli validar. OKA
        public bool VerifyPassword(UserModel usuario, string password)
        {
            var pass = new PasswordHasher<UserModel>();

            var result = pass.VerifyHashedPassword(usuario, usuario.PasswordHash, password);

            return result == PasswordVerificationResult.Success;
        }



    }
}
