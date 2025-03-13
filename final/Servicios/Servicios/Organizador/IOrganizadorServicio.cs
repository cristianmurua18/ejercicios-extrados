using Entidades.DTOs;
using Entidades.DTOs.Cruds;
using Entidades.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Servicios.Servicios.Organizador
{
    public interface IOrganizadorServicio
    {
        public Task<List<Usuario>> VerListadoUsuarios(string rol);
        public Task<bool> RegistrarJuez(CrudUsuarioDTO usuario);
        public Task<bool> CrearTorneo(CrudTorneoDTO torneo);
        public Task<bool> CrearTorneoSerieHabilitada(CrudTorneoSerieHabilitadaDTO serie);
        public Task<bool> EditarTorneo(CrudTorneoDTO torneo);
        public Task<bool> CancelarTorneo(int idtorneo, string estado);

        public Task GenerarRondasYPartidas(int idTorneo);
        public Task<bool> CrearRondas(int idTorneo);
        public Task<bool> ModificarPartida(PartidaDTO partida);
    }
}
