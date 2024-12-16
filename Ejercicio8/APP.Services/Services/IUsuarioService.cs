using APP.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APP.Services.Services
{
    public interface IUsuarioService
    {

        //Firma de los metodos que se implementen en la clase UserService


        //public bool Validate(Usuario user)
        //{
        //    return false;
        //}

        public List<Usuario> GetAll();

        public Usuario GetByEmailAddress(string email);

        public void AddUser(Usuario usuario);

        public string UpdateUserByID(Usuario usuario);

        public string DeleteUserByID(Usuario usuario);

        public string GenerateJwt(Usuario user);

        public bool Authenticate(Usuario loginUser, string password);

        public bool VerifyPassword(Usuario usuario, string password);


    }
}
