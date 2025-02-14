using AccesoDatos.DAOs;
using Azure.Identity;
using Entidades.DTOs;
using Entidades.DTOs.Jugadores;
using Entidades.DTOs.Respuestas;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using Utilidades.Utilidades;
using static System.Net.WebRequestMethods;

namespace Servicios.Servicios
{
    public class AccesoServicio(IDAOAcceso daoacceso, IComunes common, HttpClient httpClient) : IAccesoServicio
    {
        private readonly IDAOAcceso _daoAcceso = daoacceso;

        private readonly IComunes _common = common;

        private readonly HttpClient _httpClient = httpClient; //Para hacer solicitudes a apis


        //Seriamos el FronEnd

        //Implementacion de Metodos

        public async Task<bool> ObtenerPokemones()
        {
            //Llamo a la URL, la lleve al appsettings - VER OFFSET
            //string url = "https://pokeapi.co/api/v2/pokemon/";

            //Esperar que se ejecuta de Forma asincrona y que espere el resultado

            var i = 251;
            //FALTA EJECUTAR PARA TRAER DEL 251 al 300
            while (i < 301)
            {
                var res = await _httpClient.GetAsync($"https://pokeapi.co/api/v2/pokemon/{i}");

                _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                if (res.IsSuccessStatusCode)
                {
                    //var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                    //obtener el contenido
                    var body = await res.Content.ReadAsStringAsync();

                    var pokemon = JsonConvert.DeserializeObject<PokemonDTO>(body);

                    await _daoAcceso.ObtenerPokemones(pokemon!);

                    i++;

                }
            }
            return true;
        }

        public async Task<bool> RellenarCartaSerie()
        {
            //Llamo a la URL, la lleve al appsettings - VER OFFSET
            //string url = "https://pokeapi.co/api/v2/pokemon/";

            //Esperar que se ejecuta de Forma asincrona y que espere el resultado

            var i = 251;
            //Traer del 251 al 300
            while (i < 301)
            {
                var res = await _httpClient.GetAsync($"https://pokeapi.co/api/v2/pokemon/{i}");

                _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                if (res.IsSuccessStatusCode)
                {
                    //var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                    //obtener el contenido
                    var body = await res.Content.ReadAsStringAsync();

                    var pokemon = JsonConvert.DeserializeObject<CartaSerieDTO>(body);

                    await _daoAcceso.RellenarCartaSerie(pokemon!);

                    i++;
                }
            }
            return true;
        }


        public async Task<int> ObtenerIdPais(string nombre)
        {
            var idPais = await _daoAcceso.ObtenerIdPais(nombre);
            
                return idPais > 0 ? idPais : 0;

        }

        public async Task<bool> RegistroJugador(InscripcionJugadorDTO jugador)
        {
            if (jugador.Rol == "Jugador")
            {
                //Encripto contraseña al insertar Jugador
                jugador.Contraseña = _common.EncriptarSHA256(jugador.Contraseña!);

                return await _daoAcceso.RegistroJugador(jugador);

            }
            return false;

        }


        public async Task<AutorizacionRespuestaDTO> ObtenerAutenticacion(LoginDTO login)
        {
            //Encripto contraseña para el login, asi puede comparar
            login.Contraseña = _common.EncriptarSHA256(login.Contraseña!);

            var usuario = await _daoAcceso.ObtenerAutenticacion(login);

            if (usuario != null)
            {
                var token = _common.GenerarJWT(usuario);

                return new AutorizacionRespuestaDTO { Token = token, Msj = "Usuario Autorizado", Resultado = true };
            }

            return new AutorizacionRespuestaDTO { Token =  "No es posible obtenerlo", Msj = "Usuario NO Autorizado", Resultado = false };

        }




    }
}
