using Entidades.DTOs;
using Entidades.DTOs.Jugadores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccesoDatos.DAOs
{
    public interface IDAOAcceso
    {

        //Definicion de Metodos
        public Task<int> ObtenerPokemones(PokemonDTO pokemon);
        public Task<int> RellenarCartaSerie(CartaSerieDTO cartaSerie);
        public Task<int> ObtenerIdPais(string nombre);
        public Task<bool> RegistroJugador(InscripcionJugadorDTO jugador);
        public Task<UsuarioDTO> ObtenerAutenticacion(LoginDTO usuario);


    }
}
