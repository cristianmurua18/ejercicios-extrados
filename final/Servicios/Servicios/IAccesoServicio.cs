using Entidades.DTOs;
using Entidades.DTOs.Jugadores;
using Entidades.DTOs.Respuestas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Servicios.Servicios
{
    public interface IAccesoServicio
    {
        //Definicion de Metodos
        public Task<bool> ObtenerPokemones();
        public Task<bool> RellenarCartaSerie();
        public Task<int> ObtenerIdPais(string nombre);
        public Task<bool> RegistroJugador(InscripcionJugadorDTO jugador);
        public Task<AutorizacionRespuestaDTO> ObtenerAutenticacion(LoginDTO login);


    }
}
