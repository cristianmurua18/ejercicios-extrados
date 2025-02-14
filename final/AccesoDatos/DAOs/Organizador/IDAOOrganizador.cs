using Entidades.DTOs;
using Entidades.DTOs.Cruds;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccesoDatos.DAOs.Organizador
{
    public interface IDAOOrganizador
    {
        public Task<bool> RegistrarJuez(CrudUsuarioDTO usuario);
        public Task<bool> CrearTorneo(CrudTorneoDTO torneo);
        public Task<bool> EditarTorneo(CrudTorneoDTO torneo);
        public Task<bool> CancelarTorneo(int idtorneo, string estado);
        public Task<bool> CrearPartida(PartidaDTO partida);
        public Task<bool> ModificarPartida(PartidaDTO partida);

    }
}
