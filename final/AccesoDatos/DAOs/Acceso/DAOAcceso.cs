using Dapper;
using Entidades.DTOs;
using Entidades.DTOs.Cruds;
using Entidades.DTOs.Respuestas;
using System.Data;
using Utilidades.Utilidades;

namespace AccesoDatos.DAOs.Acceso
{
    public class DAOAcceso(IDbConnection dbConnection) : IDAOAcceso
    {

        private readonly IDbConnection _dbConnection = dbConnection;

        #region Metodos para el acceso sin autenticacion

        //Cambiar modelos por dto?

        //public async Task<int> CambiarContrasena(string nuevaContraseña, int userId)
        //{
        //    var sqlUpdate = @"UPDATE Usuarios
        //        SET Contraseña = @nuevaContraseña
        //        WHERE UsuarioID=@userId;";

        //    var affectedRows = await _dbConnection.ExecuteAsync(sqlUpdate, new { nuevaContraseña, userId });

        //    return affectedRows;

        //}
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

        public async Task<int> RellenarCartaSerie(CartaSerieDTO cartaSerie, int idCarta)
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
                            VALUES(@idCarta,@idSerie);";

                //Por cada tipo de pokemon, hago un insert
                count += await _dbConnection.ExecuteAsync(sqlInsert, new
                {
                    idCarta,
                    idSerie
                });

            }
            return count;
        }

        public async Task<List<TorneoDTO>> VerInfoTorneos()
        {
            var sqlSelect = @"SELECT TorneoID, NombreTorneo, FyHInicioT, Estado FROM Torneos;";

            var consulta = await _dbConnection.QueryAsync<TorneoDTO>(sqlSelect, new { });

            return consulta.ToList();
        }


        public async Task<List<RespuestaPaisDTO>> ObtenerIdPais(string nombre)
        {
            var sqlSelect = @"SELECT PaisID, NombrePais FROM Paises 
                WHERE NombrePais Like @NombrePais;";

            var consulta = await _dbConnection.QueryAsync<RespuestaPaisDTO>(sqlSelect, new { NombrePais = "%" + nombre + "%" });

            return consulta.ToList();

            //Ver exepciones por problemas con el servidor

        }

        public async Task<List<RespuestaPaisDTO>> ObtenerPaginacionPaises(int desdePagina, int cantRegistros)
        {
            var paginas = await _dbConnection.QueryAsync<RespuestaPaisDTO>(
                sql: "dbo.PaginacionPaises",
                param: new { Pag = desdePagina, Reg = cantRegistros },
                commandType: CommandType.StoredProcedure);

            return paginas.ToList();


        }

        //VER COMO LIMITAR EL REGISTRO EN ALGUN MOMENTO
        public async Task<bool> RegistroJugador(InsertarJugadorDTO jugador)
        {
            
            //Inserto el usuario en la tabla Usuario
            var sqlInsert = @"INSERT
                INTO Usuarios(NombreApellido,Alias,IdPaisOrigen,Email,NombreUsuario,Contraseña,Rol,Activo)
                VALUES(@NombreApellido,@Alias,@IdPaisOrigen,@Email,@NombreUsuario,@Contraseña,@Rol,@Activo);";

            var affectRows = await _dbConnection.ExecuteAsync(sqlInsert, jugador);

            return affectRows > 0;

        }

        //public async Task<bool> RegistroJugador(CrudUsuarioDTO jugador, int idTorneoRef)
        //{
        //    //CREAR UN TRNSACCION 
        //    try
        //    {
        //        using (var tran = _dbConnection.BeginTransaction())
        //        {  //Inserto el usuario en la tabla Usuario
        //            var sqlInsert = @"INSERT
        //            INTO Usuarios(NombreApellido,Alias,IdPaisOrigen,Email,NombreUsuario,Contraseña,FotoAvatar,Rol,CreadoPor,Activo)
        //            OUTPUT INSERTED.UsuarioID
        //            VALUES(@NombreApellido,@Alias,@IdPaisOrigen,@Email,@NombreUsuario,@Contraseña,@FotoAvatar,@Rol,@CreadoPor,@Activo);";
        //            //Aqui debo recuperar el ID del Nuevo Usuario, al hacer el insert
        //            var userId = await _dbConnection.ExecuteScalarAsync<int>(sqlInsert, jugador, transaction: tran);
        //            //si es mayor a 0, significa que fue correcta la insercion 
        //            if (userId > 0)
        //            {   //Traigo info del torneo
        //                var sqlSelect = @"SELECT FyHInicioT, Estado, PartidasDiarias, DiasDeDuracion FROM Torneos
        //                    where TorneoID = @idTorneoRef;";

        //                var torneo = await _dbConnection.QuerySingleOrDefaultAsync<TorneoDTO>(sqlSelect, new { idTorneoRef }, transaction: tran);
        //               //Si existe y su estado es Registro. IDEA: Corrobar que fecha de inicio sea menor a hoy. O sea si no comenzo.
        //                if (torneo != null && torneo.Estado == "Registro")
        //                {
        //                    var select = @"SELECT COUNT(IdJugador) from InfoTorneoJugadores where IdTorneo = @idTorneo;;";
        //                    //Traigo info de los inscriptos hasta el momento
        //                    var inscriptos = await _dbConnection.ExecuteScalarAsync<int>(select, new { idTorneoRef }, transaction: tran);

        //                    if (inscriptos < 2)
        //                        throw new InvalidOperationException("No es posible iniciar el torneo con solo un jugador");
        //                    //El maximo de jugadores en primera ronda
        //                    var maxJugadores = torneo.PartidasDiarias * torneo.DiasDeDuracion;

        //                    if(inscriptos <= maxJugadores)
        //                    {
        //                        //Lo inscribo en el torneo, luego el organizador va a asignarle su mazo de cartas.
        //                        var sqlInsertar = @"INSERT
        //                            INTO InfoTorneoJugadores(IdTorneo,IdJugador) 
        //                            VALUES(@TorneoID,@userId);";

        //                        var insert = await _dbConnection.ExecuteAsync(sqlInsertar, new { torneo.TorneoID, userId }, transaction: tran);

        //                        if (insert>0)
        //                        {   //Cuando se llegue al numero maximo de inscriptos se cambia el estado del Torneo
        //                            if (inscriptos == maxJugadores)
        //                                torneo.Estado = "Partidas";
        //                            tran.Commit();
        //                            return true;
        //                        }

        //                        else
        //                        {
        //                            tran.Rollback();
        //                            throw new InvalidOperationException("Registro fallido. No fue posible inscribir al jugador.");
        //                        }
        //                    }
        //                    else
        //                        throw new InvalidOperationException("Registro fallido. Se ha llegado al maximo posible de inscriptos.");
        //                }
        //                else
        //                    throw new InvalidOperationException("Registro fallido. El torneo no existe o no esta en etapa de Registro.");
        //            }
        //            tran.Rollback();
        //            throw new InvalidOperationException("Registro fallido. No se pudo inscribir el jugador por error en sus datos.");
                    
        //            //Ver exepciones por problemas con el servidor
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine(ex.Message);
        //        return false;
        //    }
        //}
        

        //IDEA: A lo mejor se puede hacer una consulta mas especifica a la informacion necesaria, y no devolver todo
        //us.UsuarioID, us.NombreApellido, us.Email, us.NombreUsuario, us.Contraseña, us.Rol, pa.NombrePais
        public async Task<UsuarioDTO> ObtenerAutenticacion(LoginDTO usuario)
        {   //SI O SI JOIN
            string sqlJoin =
                @"SELECT *
                FROM Usuarios us
                INNER JOIN Paises pa
                ON us.IdPaisOrigen = pa.PaisID
                WHERE us.NombreUsuario=@NombreUsuario AND us.Contraseña=@Contraseña AND Activo=1;";
            //Seguir REVISAR, para no traer datos al pedo
            var registrado = await _dbConnection.QuerySingleOrDefaultAsync<UsuarioDTO>(sqlJoin, new { usuario.NombreUsuario, usuario.Contraseña });

            return registrado!;
        }


        #endregion Final 

    }
}
