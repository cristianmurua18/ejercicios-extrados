using Entidades.DTOs;
using Entidades.DTOs.Cruds;

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
        public Task<List<TorneoDTO>> VerTorneosYPartidas();
        public Task<bool> CancelarTorneos(int torneoid, string texto);
       

    }
}
