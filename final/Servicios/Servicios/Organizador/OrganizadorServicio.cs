using AccesoDatos.DAOs.Organizador;
using Entidades.DTOs;
using Entidades.DTOs.Cruds;
using Entidades.DTOs.Respuestas;
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


        public async Task<List<UsuarioPaisDTO>> VerListadoUsuarios(string rol)
        {
            var miUsuario = Convert.ToInt32(_httpContextAccessor.HttpContext?.User.FindFirst("UsuarioID")?.Value);
            return await _daoOrganizador.VerListadoUsuarios(rol, miUsuario);
        }

        public async Task<bool> RegistrarJuez(InsertarJuezDTO juez)
        {   //Encripto contraseña
            var miUsuario = Convert.ToInt32(_httpContextAccessor.HttpContext?.User.FindFirst("UsuarioID")?.Value);
            juez.Contraseña = _common.EncriptarSHA256(juez.Contraseña!);
            return await _daoOrganizador.RegistrarJuez(juez, miUsuario);
        }

        public async Task<bool> AsignarJuezATorneo(int idJuez, int idTorneo)
        {
            var torneo = await _daoOrganizador.TraerTorneo(idJuez, idTorneo);

            var juez = await _daoOrganizador.VerUsuario(idJuez);

            //Controlo que sea el organizador del torneo y que existe el juez
            if (torneo == null || juez == null)
                return false;
            return await _daoOrganizador.AsignarJuezATorneo(idJuez, idTorneo);
        }
        public async Task<bool> CrearTorneo(InsertarTorneoDTO torneo)
        {
            var miUsuario = Convert.ToInt32(_httpContextAccessor.HttpContext?.User.FindFirst("UsuarioID")?.Value);
            return await _daoOrganizador.CrearTorneo(torneo, miUsuario);
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
            var miUsuario = Convert.ToInt32(_httpContextAccessor.HttpContext?.User.FindFirst("UsuarioID")?.Value);
            var torneo = await _daoOrganizador.TraerTorneo(idTorneo, miUsuario);

            if (torneo == null)
                return false;

            return await _daoOrganizador.CerrarInscrpcionTorneo(idTorneo);
        }
        public async Task<bool> GenerarRondasYPartidas(int idTorneo)
        {
            var organizador = Convert.ToInt32(_httpContextAccessor.HttpContext?.User.FindFirst("UsuarioID")?.Value);
            
            //Pasar jugadores para calcular partidas y cuantos van a sobrar en primera ronda
            return await _daoOrganizador.GenerarRondasYPartidas(organizador, idTorneo);
        }

        public async Task<string> VerRondasYPartidas(int idTorneo)
        {
            var miUsuario = Convert.ToInt32(_httpContextAccessor.HttpContext?.User.FindFirst("UsuarioID")?.Value);
            var torneo = await _daoOrganizador.TraerTorneo(idTorneo, miUsuario);

            if (torneo == null)
                return "El torneo no es valido o no tienes permiso";
            else
            { 
                var mensaje = string.Empty;

                var listado = await _daoOrganizador.VerRondasYPartidas(idTorneo);

                foreach (var item in listado)
                {
                    mensaje += $"PartidaID: {item.PartidaID}, RondaId: {item.RondaID}, {item.JugadorUno}, {item.JugadorDos} ";
                    //SEGUIR
                }
                return mensaje;
            }



        }
        public async Task<bool> AvanzarRonda(int idTorneo, int idRonda)
        {
            var miUsuario = Convert.ToInt32(_httpContextAccessor.HttpContext?.User.FindFirst("UsuarioID")?.Value);
            var torneo = await _daoOrganizador.TraerTorneo(idTorneo, miUsuario);

            if (torneo == null)
                return false;

            return await _daoOrganizador.AvanzarRonda(idTorneo, idRonda);
        }
    }
}
