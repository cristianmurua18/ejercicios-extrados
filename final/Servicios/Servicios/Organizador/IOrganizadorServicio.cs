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
        public Task<string> VerInscriptosByTorneo(int idTorneo);
        public Task<bool> EliminarInscriptoByTorneo(int idJugador);
        public Task<List<UsuarioPaisDTO>> VerListadoUsuarios(string rol);
        public Task<bool> RegistrarJuez(InsertarJuezDTO juez);
        public Task<bool> AsignarJuezATorneo(int idJuez, int idTorneo);
        public Task<bool> CrearTorneo(InsertarTorneoDTO torneo);
        public Task<bool> CrearTorneoSerieHabilitada(CrudTorneoSerieHabilitadaDTO serie);
        public Task<bool> EditarTorneo(CrudTorneoDTO torneo);
        public Task<bool> CancelarTorneo(int idtorneo, string estado);
        public Task<bool> CerrarInscrpcionTorneo(int idTorneo);
        public Task<bool> GenerarRondasYPartidas(int idTorneo);
        public Task<string> VerRondasYPartidas(int idTorneo);
        public Task<bool> AvanzarRonda(int idTorneo, int idRonda);
    }
}
