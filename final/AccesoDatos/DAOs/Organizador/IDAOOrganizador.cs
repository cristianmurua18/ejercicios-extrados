using Entidades.DTOs;
using Entidades.DTOs.Cruds;
using Entidades.DTOs.Respuestas;
using Entidades.DTOs.Varios;
using Entidades.Modelos;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccesoDatos.DAOs.Organizador
{
    public interface IDAOOrganizador
    {
        //Solo lo uso en el servicio
        public Task<List<TorneoJugadorDTO>> VerInscriptosByTorneo(int idTorneo);
        public Task<bool> EliminarInscriptoByTorneo(int idJugador);
        public Task<int> ContarInscriptosByTorneo(int idTorneo);
        public Task<int> ContarInscriptosByTorneo(int idTorneo, IDbTransaction transaction);
        public Task<TorneoDTO> TraerTorneo(int organizador, int idTorneo);
        public Task<TorneoDTO> TraerTorneo(int organizador, int idTorneo, IDbTransaction transaction);
        public Task<UsuarioDTO> VerUsuario(int id);
        public Task<List<UsuarioPaisDTO>> VerListadoUsuarios(string rol, int miUsuario);
        public Task<bool> RegistrarJuez(InsertarJuezDTO juez, int miUsuario);
        public Task<bool> AsignarJuezATorneo(int idJuez, int idTorneo);
        public Task<bool> CrearTorneo(InsertarTorneoDTO torneo, int miUsuario);
        public Task<bool> CrearTorneoSerieHabilitada(CrudTorneoSerieHabilitadaDTO serie);
        public Task<bool> EditarTorneo(CrudTorneoDTO torneo);
        public Task<bool> CancelarTorneo(int idtorneo, string estado);
        public Task<bool> CerrarInscrpcionTorneo(int idTorneo);
        public Task<bool> GenerarRondasYPartidas(int organizador, int torneoId);
        public Task<List<PartidaRondaDTO>> VerRondasYPartidas(int idTorneo);
        public Task<bool> AvanzarRonda(int idTorneo, int idRonda);

    }
}
