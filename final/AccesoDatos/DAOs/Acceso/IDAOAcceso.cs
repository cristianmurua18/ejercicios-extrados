using Entidades.DTOs;
using Entidades.DTOs.Cruds;
using Entidades.DTOs.Jugadores;
using Entidades.DTOs.Respuestas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccesoDatos.DAOs.Acceso
{
    public interface IDAOAcceso
    {

        //Definicion de Metodos
        public Task<int> ObtenerPokemones(PokemonDTO pokemon);
        public Task<int> RellenarCartaSerie(CartaSerieDTO cartaSerie);
        public Task<List<RespuestaPaisDTO>> ObtenerIdPais(string nombre);
        public Task<bool> RegistroJugador(CrudUsuarioDTO jugador);
        public Task<UsuarioDTO> ObtenerAutenticacion(LoginDTO usuario);


    }
}
