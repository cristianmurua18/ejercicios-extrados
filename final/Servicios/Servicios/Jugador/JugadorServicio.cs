using AccesoDatos.DAOs.Jugador;
using Entidades.DTOs.Cruds;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Servicios.Servicios.Jugador
{
    public class JugadorServicio(IDAOJugador daoJugador, IHttpContextAccessor httpContextAccessor) : IJugadorServicio
    {

        private readonly IDAOJugador _daoJugador = daoJugador;

        //Es para traer la info de los claims del usuario logeado
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

        public async Task<int> CrearMazo(string nombreMazo)
        {
            //Traigo el id del usuario al autenticarse
            var userIdClaim = _httpContextAccessor.HttpContext?.User.FindFirst("UsuarioID")?.Value;

            var userId = int.Parse(userIdClaim!);

            return await _daoJugador.CrearMazo(userId,nombreMazo);

        }
    
        public async Task<bool> RegistrarCartas(CrudMazoCartasDTO cartas, int idTorneo)
        {
            //Traigo el id del usuario al autenticarse
            var userIdClaim = _httpContextAccessor.HttpContext?.User.FindFirst("UsuarioID")?.Value;
            var userId = int.Parse(userIdClaim!);

            return await _daoJugador.RegistrarCartas(cartas, idTorneo, userId);

        }

    }
}
