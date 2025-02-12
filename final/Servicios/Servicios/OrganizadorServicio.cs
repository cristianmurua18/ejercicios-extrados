using AccesoDatos.DAOs;
using Entidades.DTOs;
using Entidades.DTOs.Cruds;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Servicios.Servicios
{
    public class OrganizadorServicio(IDAOOrganizador daoOrganizador) : IOrganizadorServicio
    {

        private readonly IDAOOrganizador _daoOrganizador = daoOrganizador;


        public async Task<bool> RegistrarJuez(CrudUsuarioDTO usuario)
        {
            return await _daoOrganizador.RegistrarJuez(usuario);
        }
        public async Task<bool> CrearTorneo(CrudTorneoDTO torneo)
        {
            return await _daoOrganizador.CrearTorneo(torneo);
        }

        public async Task<bool> EditarTorneo(CrudTorneoDTO torneo)
        {
            return await _daoOrganizador.EditarTorneo(torneo);
        }

        public async Task<bool> CancelarTorneo(int idtorneo, string estado)
        {
            return await _daoOrganizador.CancelarTorneo(idtorneo, estado);
        }


        public async Task<bool> CrearPartida(PartidaDTO partida)
        {
            return await _daoOrganizador.CrearPartida(partida);
        }

        public async Task<bool> ModificarPartida(PartidaDTO partida)
        {
            return await _daoOrganizador.ModificarPartida(partida);
        }
    }
}
