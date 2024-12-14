using APP.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace APP.Entities.Daos
{
    public interface IDAOUsuario
    {
        public List<Usuario> ObtenerListadoUsuarios();

        public Usuario ObtenerUsuarioPorEmail(string email);

        public void AgregarUsuario(Usuario usuario);

        public string ModificarUsuarioPorID(Usuario usuario);

        public string BorrarUsuarioPorID(Usuario usuario);

        public string HashPassword<T>(T usuario) where T : class;


    }
}
