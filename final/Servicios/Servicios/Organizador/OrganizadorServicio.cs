using AccesoDatos.DAOs.Organizador;
using Entidades.DTOs;
using Entidades.DTOs.Cruds;
using Entidades.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilidades.Utilidades;

namespace Servicios.Servicios.Organizador
{
    public class OrganizadorServicio(IDAOOrganizador daoOrganizador, IComunes comunes) : IOrganizadorServicio
    {

        private readonly IDAOOrganizador _daoOrganizador = daoOrganizador;

        private readonly IComunes _common = comunes;


        public async Task<List<Usuario>> VerListadoUsuarios(string rol)
        {
            return await _daoOrganizador.VerListadoUsuarios(rol);
        }

        public async Task<bool> RegistrarJuez(CrudUsuarioDTO usuario)
        {   //Encripto contraseña
            usuario.Contraseña = _common.EncriptarSHA256(usuario.Contraseña!);
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
