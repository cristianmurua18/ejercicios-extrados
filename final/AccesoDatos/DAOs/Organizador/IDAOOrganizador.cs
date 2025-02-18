using Entidades.DTOs;
using Entidades.DTOs.Cruds;
using Entidades.Modelos;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccesoDatos.DAOs.Organizador
{
    public interface IDAOOrganizador
    {
        //Solo lo uso en el servicio
        public Task<int> ContarInscriptosActivos();
        public Task<TorneoDTO> TraerTorneo(int organizador, int idTorneo);
        public Task<List<Usuario>> VerListadoUsuarios(string rol);
        public Task<bool> RegistrarJuez(CrudUsuarioDTO usuario);
        public Task<bool> CrearTorneo(CrudTorneoDTO torneo);
        public Task<bool> CrearTorneoSerieHabilitada(CrudTorneoSerieHabilitadaDTO serie);
        public Task<bool> EditarTorneo(CrudTorneoDTO torneo);
        public Task<bool> CancelarTorneo(int idtorneo, string estado);
        public Task<bool> CrearPartida(PartidaDTO partida);
        public Task<bool> ModificarPartida(PartidaDTO partida);

    }
}
