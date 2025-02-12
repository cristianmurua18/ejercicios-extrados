using Entidades.DTOs;
using Entidades.DTOs.Cruds;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Servicios.Servicios
{
    public interface IAdministradorServicio
    {
        public Task<List<UsuarioDTO>> ObtenerUsuariosPorNombre(string patron);
        public Task<UsuarioDTO> ObtenerUsuarioPorId(int id);
        public Task<bool> RegistrarUsuario(CrudUsuarioDTO usuario);
        public Task<bool> ActualizarUsuarioPorID(CrudUsuarioDTO usuario);
        public Task<bool> BorrarUsuarioPorID(CrudUsuarioDTO usuario);
        public Task<List<TorneoDTO>> VerTorneosYpartidas();
        public Task<string> CancelarTorneos(int torneoid, string texto);
        
        //public Task<bool> DescalificarJugador(JugadorDescalificadoDTO jugadorDescalificado);



    }
}
