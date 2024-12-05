using APP.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APP.Service.Services
{
    public interface IUserService
    {
        //Firma de los metodos que se implementen en la clase UserService


        //public bool Validate(UserModel user)
        //{
        //    return false;
        //}

        public List<UserModel> GetLogin();

        public List<UserModel> GetAll();

        public UserModel GetByEmailAddress(string email);

        public string AddUser(UserModel usuario);

        public string UpdateUserByID(UserModel usuario);

        public string DeleteUserByID(UserModel usuario);

        public string GenerateJwt(UserModel user);

        public bool Authenticate(UserModel loginUser, string password);

        public bool VerifyPassword(UserModel usuario, string password);


        //Falta el hasheo de contraseña

        //Verificacion de la contra hashaeada


    }
}
