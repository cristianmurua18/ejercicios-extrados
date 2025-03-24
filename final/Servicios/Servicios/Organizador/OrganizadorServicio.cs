using AccesoDatos.DAOs.Organizador;
using Entidades.DTOs;
using Entidades.DTOs.Cruds;
using Entidades.Modelos;
using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilidades.Utilidades;

namespace Servicios.Servicios.Organizador
{
    public class OrganizadorServicio(IDAOOrganizador daoOrganizador, IComunes comunes, IHttpContextAccessor httpContextAccessor) : IOrganizadorServicio
    {
        //Es para traer la info de los claims del usuario logeado
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

        private readonly IDAOOrganizador _daoOrganizador = daoOrganizador;

        private readonly IComunes _common = comunes;

        public async Task<string> VerInscriptosByTorneo(int idTorneo)
        {
            var mensaje = string.Empty;
            var inscriptos = await _daoOrganizador.VerInscriptosByTorneo(idTorneo);

            if (inscriptos == null || inscriptos.Count() == 0)
            {
                return mensaje = "Todavia no hay inscriptos";
            }
            else
            {
                foreach (var jugador in inscriptos)
                {
                    mensaje += $"Id Jugador : {jugador.IdJugador}, Id Mazo: {jugador.IdMazo}";
                }
                return mensaje;
            }

        }
        public async Task<bool> EliminarInscriptoByTorneo(int idJugador)
        {
            return await _daoOrganizador.EliminarInscriptoByTorneo(idJugador);
        }


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

        public async Task<bool> CrearTorneoSerieHabilitada(CrudTorneoSerieHabilitadaDTO serie)
        {
            return await _daoOrganizador.CrearTorneoSerieHabilitada(serie);
        }

        public async Task<bool> EditarTorneo(CrudTorneoDTO torneo)
        {
            return await _daoOrganizador.EditarTorneo(torneo);
        }

        public async Task<bool> CancelarTorneo(int idtorneo, string estado)
        {
            return await _daoOrganizador.CancelarTorneo(idtorneo, estado);
        }

        public async Task<bool> CerrarInscrpcionTorneo(int idTorneo)
        {
            return await _daoOrganizador.CerrarInscrpcionTorneo(idTorneo);
        }
        public async Task<bool> GenerarRondasYPartidas(int idTorneo)
        {
            var organizador = Convert.ToInt32(_httpContextAccessor.HttpContext?.User.FindFirst("UsuarioID")?.Value);
            
            //Pasar jugadores para calcular partidas y cuantos van a sobrar en primera ronda
            return await _daoOrganizador.GenerarRondasYPartidas(organizador, idTorneo);
        }


        public async Task<bool> ModificarPartida(PartidaDTO partida)
        {
            return await _daoOrganizador.ModificarPartida(partida);
        }
    }
}
