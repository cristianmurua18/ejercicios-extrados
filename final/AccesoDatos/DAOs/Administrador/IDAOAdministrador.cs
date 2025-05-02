using Entidades.DTOs;
using Entidades.DTOs.Cruds;
using Entidades.DTOs.Varios;

namespace AccesoDatos.DAOs.Administrador
{
    public interface IDAOAdministrador
    {
        public Task<List<UsuarioDTO>> ObtenerUsuariosPorRol(string rol);
        public Task<List<UsuarioDTO>> ObtenerUsuariosPorNombre(string nombre);
        public Task<UsuarioDTO> ObtenerUsuarioPorId(int id);
        public Task<bool> RegistrarUsuario(CrudUsuarioDTO usuario);
        public Task<bool> ActualizarUsuarioPorID(CrudUsuarioDTO usuario);
        public Task<bool> BorrarUsuarioPorID(int id);
        public Task<List<TorneoDTO>> VerTorneos();
        public Task<bool> CancelarTorneo(int torneoid);
       

    }
}
