using Entidades.DTOs;
using Entidades.DTOs.Cruds;
using Entidades.DTOs.Varios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Servicios.Servicios.Administrador
{
    public interface IAdministradorServicio
    {
        public Task<List<UsuarioDTO>> ObtenerUsuariosPorRol(string rol);
        public Task<List<UsuarioDTO>> ObtenerUsuariosPorNombre(string nombre);
        public Task<UsuarioDTO> ObtenerUsuarioPorId(int id);
        public Task<bool> RegistrarUsuario(CrudUsuarioDTO usuario);
        public Task<bool> ActualizarUsuarioPorID(CrudUsuarioDTO usuario);
        public Task<bool> BorrarUsuarioPorID(int id);
        public Task<List<TorneoDTO>> VerTorneos();
        public Task<bool> CancelarTorneo(int torneoid);
        
        //public Task<bool> DescalificarJugador(JugadorDescalificadoDTO jugadorDescalificado);



    }
}
