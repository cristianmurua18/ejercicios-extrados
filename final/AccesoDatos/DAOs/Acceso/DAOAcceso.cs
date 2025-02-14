using Dapper;
using Entidades.DTOs;
using System.Data;
using System.Net.Http.Headers;
using System.Net.Http;
using Entidades.DTOs.Jugadores;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Utilidades.Utilidades;

namespace AccesoDatos.DAOs.Acceso
{
    public class DAOAcceso(IDbConnection dbConnection) : IDAOAcceso
    {

        private readonly IDbConnection _dbConnection = dbConnection;

        #region Metodos para el acceso sin autenticacion

        //Cambiar modelos por dto?

        public async Task<int> ObtenerPokemones(PokemonDTO pokemon)
        {
            var sqlInsert = @"INSERT
                            INTO Cartas(NombreCarta,Ataque, Defensa, Ilustracion) 
                            VALUES(@NombreCarta,@Ataque,@Defensa,@Ilustracion);";

            //Por cada pais, hago un insert
            var res = await _dbConnection.ExecuteAsync(sqlInsert, new
            {
                NombreCarta = pokemon.Name!,
                Ataque = pokemon.Height!,
                Defensa = pokemon.Weight!,
                Ilustracion = pokemon.Url
            });             
            return res;
        }

        public async Task<int> RellenarCartaSerie(CartaSerieDTO cartaSerie)
        {
            var count = 0;

            var idSerie = 0;

            foreach (var item in cartaSerie.types)
            {
                idSerie = item.type!.name switch
                {
                    "normal" => Convert.ToInt32(SeriesEnum.normal),
                    "fire" => Convert.ToInt32(SeriesEnum.fire),
                    "water" => Convert.ToInt32(SeriesEnum.water),
                    "grass" => Convert.ToInt32(SeriesEnum.grass),
                    "electric" => Convert.ToInt32(SeriesEnum.electric),
                    "ice" => Convert.ToInt32(SeriesEnum.ice),
                    "fighting" => Convert.ToInt32(SeriesEnum.fighting),
                    "poison" => Convert.ToInt32(SeriesEnum.poison),
                    "ground" => Convert.ToInt32(SeriesEnum.ground),
                    "flying" => Convert.ToInt32(SeriesEnum.flying),
                    "psychic" => Convert.ToInt32(SeriesEnum.psychic),
                    "bug" => Convert.ToInt32(SeriesEnum.bug),
                    "rock" => Convert.ToInt32(SeriesEnum.rock),
                    "ghost" => Convert.ToInt32(SeriesEnum.ghost),
                    "dark" => Convert.ToInt32(SeriesEnum.dark),
                    "dragon" => Convert.ToInt32(SeriesEnum.dragon),
                    "steel" => Convert.ToInt32(SeriesEnum.steel),
                    "fairy" => Convert.ToInt32(SeriesEnum.fairy),
                    _ => 0,
                };

                var sqlInsert = @"INSERT
                            INTO CartaSerie(IdCarta,IdSerie) 
                            VALUES(@id,@idSerie);";

                //Por cada tipo de pokemon, hago un insert
                count += await _dbConnection.ExecuteAsync(sqlInsert, new
                {
                    cartaSerie.id, idSerie
                });
                
            }
            return count;
        }

        public async Task<int> ObtenerIdPais(string nombre)
        {
            var sqlSelect = @"SELECT PaisID FROM Paises 
                WHERE NombrePais Like @NombrePais;";

            var consulta = await _dbConnection.ExecuteScalarAsync<int>(sqlSelect, new { NombrePais = "%" + nombre + "%" });

            return consulta;

            //Ver exepciones por problemas con el servidor

        }
        //VER COMO LIMITAR EL REGISTRO EN ALGUN MOMENTO
        public async Task<bool> RegistroJugador(InscripcionJugadorDTO jugador)
        {
            var sqlInsert = @"INSERT
                INTO Usuarios(NombreApellido,Alias,IdPaisOrigen,Email,NombreUsuario,Contraseña,FotoAvatar,Rol,CreadorPor,Activo) 
                VALUES(@NombreApellido,@Alias,@IdPaisOrigen,@Email,@NombreUsuario,@Contraseña,@FotoAvatar,@Rol,@CreadoPor,@Activo);";

            var result = await _dbConnection.ExecuteAsync(sqlInsert, jugador);

            if (result > 0)
                return true;
            return false;
            //Ver exepciones por problemas con el servidor

        }

        //Solo la uso para obtener la info del Usuario para los Claims, a lo mejor devuelvo mucha informacion al pedo
        //IDEA: A lo mejor se puede hacer una consulta mas especifica a la informacion necesaria, y no devolver todo
        public async Task<UsuarioDTO> ObtenerAutenticacion(LoginDTO usuario)
        {   //SI O SI JOIN
            string sqlJoin =
                @"SELECT *
                FROM Usuarios us
                INNER JOIN Paises pa
                ON us.IdPais = pa.PaisID
                WHERE us.NombreUsuario=@NombreUsuario AND us.Contraseña=@Contraseña;";
            //Seguir REVISAR
            var registrado = await _dbConnection.QuerySingleOrDefaultAsync<UsuarioDTO>(sqlJoin, new { });

            return registrado!;
        }


        #endregion Final 

    }
}
