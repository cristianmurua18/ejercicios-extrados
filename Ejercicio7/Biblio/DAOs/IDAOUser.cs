using APP.Entities.Models;

namespace APP.Entities.DAOs
{
    public interface IDAOUser
    {

        public List<UserModel> ObtenerListadoUsuarios();

        public UserModel ObtenerUsuarioPorEmail(string email);

        public void AgregarUsuario(UserModel usuario);

        public string ModificarUsuarioPorID(UserModel usuario);

        public string BorrarUsuarioPorID(UserModel usuario);

        //NO es posible pasar dos clases directamente, solo se puede con una
        //public string HashPassword<T>(T usuario) where T: UserModel, LoginUser;

        public string HashPassword<T>(T usuario) where T : class;



    }
}
