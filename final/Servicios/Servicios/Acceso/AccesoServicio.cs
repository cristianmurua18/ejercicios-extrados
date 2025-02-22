using AccesoDatos.DAOs.Acceso;
using Azure.Identity;
using Entidades.DTOs;
using Entidades.DTOs.Cruds;
using Entidades.DTOs.Jugadores;
using Entidades.DTOs.Respuestas;
using Entidades.Modelos;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using Utilidades.Utilidades;
using static System.Net.WebRequestMethods;

namespace Servicios.Servicios.Acceso
{
    public class AccesoServicio(IDAOAcceso daoacceso, IComunes common, HttpClient httpClient) : IAccesoServicio
    {
        private readonly IDAOAcceso _daoAcceso = daoacceso;

        private readonly IComunes _common = common;

        private readonly HttpClient _httpClient = httpClient; //Para hacer solicitudes a api
                                                              //Seriamos el FronEnd

        //Implementacion de Metodos

        //public async Task<bool> ObtenerPokemones()
        //{
        //    //Llamo a la URL, la lleve al appsettings - VER OFFSET
        //    //string url = "https://pokeapi.co/api/v2/pokemon/";

        //    //Esperar que se ejecuta de Forma asincrona y que espere el resultado

        //    var i = 1;
        //    //Traer del 1001 al 1025. No hay mas
        //    while (i < 0)
        //    {
        //        var res = await _httpClient.GetAsync($"https://pokeapi.co/api/v2/pokemon/{i}");

        //        _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        //        if (res.IsSuccessStatusCode)
        //        {
        //            //var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        //            //obtener el contenido
        //            var body = await res.Content.ReadAsStringAsync();

        //            var pokemon = JsonConvert.DeserializeObject<PokemonDTO>(body);

        //            await _daoAcceso.ObtenerPokemones(pokemon!);

        //            i++;

        //        }
        //    }
        //    return true;
        //}

        //public async Task<bool> RellenarCartaSerie()
        //{
        //    //Llamo a la URL, la lleve al appsettings - VER OFFSET
        //    //string url = "https://pokeapi.co/api/v2/pokemon/";

        //    //Esperar que se ejecuta de Forma asincrona y que espere el resultado

        //    var i = 1001;

        //    while (i < 1026)
        //    {
        //        var res = await _httpClient.GetAsync($"https://pokeapi.co/api/v2/pokemon/{i}");

        //        _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        //        if (res.IsSuccessStatusCode)
        //        {
        //            //var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        //            //obtener el contenido
        //            var body = await res.Content.ReadAsStringAsync();

        //            var pokemon = JsonConvert.DeserializeObject<CartaSerieDTO>(body);

        //            await _daoAcceso.RellenarCartaSerie(pokemon!);

        //            i++;
        //        }
        //    }
        //    return true;
        //}

        public async Task<string> VerInfoTorneos()
        {
            var mensaje = string.Empty;

            var torneos = await _daoAcceso.VerInfoTorneos();

            if (torneos == null || torneos.Count == 0)
            {
                return mensaje = "No hay torneos proximos";
            }
            foreach (var torneo in torneos)
            {
                mensaje += $"TorneoID: {torneo.TorneoID}, Nombre: {torneo.NombreTorneo}, FyHInicio: {torneo.FyHInicioT}, Estado: {torneo.Estado} \n";
            }
            return mensaje;

  
        }

        public async Task<List<RespuestaPaisDTO>> ObtenerIdPais(string nombre)
        {
            return await _daoAcceso.ObtenerIdPais(nombre);

        }

        public async Task<List<RespuestaPaisDTO>> ObtenerPaginacionPaises(int desdePagina, int cantRegistros)
        {
            return await _daoAcceso.ObtenerPaginacionPaises(desdePagina, cantRegistros);            
        }

        public async Task<bool> RegistroJugador(CrudUsuarioDTO jugador, int idTorneoRef)
        {
            if(jugador.Rol != "Jugador")
                throw new ArgumentException("Registro fallido. El usuario debe ser jugador");

            jugador.Contraseña = _common.EncriptarSHA256(jugador.Contraseña!);

            return await _daoAcceso.RegistroJugador(jugador, idTorneoRef);
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
