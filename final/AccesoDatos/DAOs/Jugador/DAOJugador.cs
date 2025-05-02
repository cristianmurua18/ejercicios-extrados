using Dapper;
using Entidades.DTOs;
using Entidades.DTOs.Cruds;
using Entidades.DTOs.Respuestas;
using Entidades.Modelos;
using System.Data;
using System.Data.Common;
using Validaciones.Excepciones;

namespace AccesoDatos.DAOs.Jugador
{
    public class DAOJugador(IDbConnection dbConnection) : IDAOJugador
    {

        private readonly IDbConnection _dbConnection = dbConnection;

        public async Task<int> CrearColeccion(int idCarta, int userId)
        {
            var sqlInsert = @"INSERT
                INTO CartasColeccionadas(IdCarta, IdUsuario)
                VALUES(@idCarta,@userId);";

            var affectedRows = await _dbConnection.ExecuteAsync(sqlInsert, new { idCarta, userId });

            return affectedRows;

        }


        //Creo un Mazo, le pongo nombre y jugador que lo creo(viene de los claims)
        public async Task<int> CrearMazo(int userId, string nombreMazo)
        {
            var sqlInsert = @"INSERT
                INTO Mazos(NombreMazo,JugadorCreador)
                OUTPUT INSERTED.MazoID
                VALUES(@NombreMazo,@JugadorCreador);";

            var mazoID = await _dbConnection.ExecuteScalarAsync<int>(sqlInsert, new { NombreMazo = nombreMazo, JugadorCreador = userId });

            return mazoID;

        }

        public async Task<List<CrudMazoDTO>> VerMisMazos(int userId)
        {
            var sqlSelect = @"SELECT * from Mazos where JugadorCreador = @userId;";

            var mazo = await _dbConnection.QueryAsync<CrudMazoDTO>(sqlSelect, new { JugadorCreador = userId });

            return mazo.ToList();

        }

        public async Task<CrudMazoDTO> ControlarMazo(int idMazo, int userId)
        {
            var sqlSelect = @"SELECT * from Mazos where MazoID = @IdMazo AND JugadorCreador = @userId;";

            var mazo = await _dbConnection.QueryFirstOrDefaultAsync<CrudMazoDTO>(sqlSelect, new { idMazo, JugadorCreador = userId });

            return mazo!;

        }

        public async Task<CartaColeccionDTO> ControlarCartaColeccionada(int userId)
        {
            var sqlSelect = @"SELECT IdCarta from CartasColeccionadas where IdUsuario = @userId;";

            var cartaColeccion = await _dbConnection.QueryFirstOrDefaultAsync<CartaColeccionDTO>(sqlSelect, new { IdUsuario = userId });

            return cartaColeccion!;

        }

        public async Task<List<RespuestaCartaSerieDTO>> ControlarCartaSerie(int idCarta, int idTorneo)
        {
            var sqlSelect = @"SELECT c.CartaID, c.NombreCarta, s.SerieID, s.NombreSerie
                            FROM CartaSerie cs
                            JOIN Cartas c ON cs.IdCarta = c.CartaID
                            JOIN Series s ON cs.IdSerie = s.SerieID
                            WHERE c.CartaID = @IdCarta AND
                            cs.IdSerie IN(SELECT IdSerie from TorneoSerieHabilitada WHERE IdTorneo = @idTorneo);";
            //Controlo que carta pertenezca a una serie que acepta un torneo de referencia
            var cartas = await _dbConnection.QueryAsync<RespuestaCartaSerieDTO>(sqlSelect, new { idCarta, idTorneo });
            
            return cartas.ToList();

        }

        public async Task<int> InsertarCartaEnMazo(int idMazo, int idCarta, IDbTransaction tran)
        {
            var sqlInsertar = @"INSERT
                                INTO MazoCartas(IdMazo,IdCarta) 
                                VALUES(@IdMazo,@IdCarta);";

            var insert = await _dbConnection.ExecuteAsync(sqlInsertar, new { idMazo, idCarta }, transaction: tran);

           return insert;

        }

        public async Task<int> ContarCartasEnMazo(int idMazo, IDbTransaction tran)
        {
            var sqlInsertar = @"SELECT COUNT(IdCarta) AS Cantidad from MazoCartas
                                WHERE MazoCartas.IdMazo = @idMazo;";

            var insert = await _dbConnection.ExecuteScalarAsync<int>(sqlInsertar, new { idMazo }, transaction: tran);

            return insert;

        }

        public async Task<int> ContarCartasEnMazo(int idMazo)
        {
            var sqlInsertar = @"SELECT COUNT(IdCarta) AS Cantidad from MazoCartas
                                WHERE MazoCartas.IdMazo = @idMazo;";

            var insert = await _dbConnection.ExecuteScalarAsync<int>(sqlInsertar, new { idMazo });

            return insert;

        }

        //El proc debe pedir en que torneo sera utilizado el mazo y se debe chequear que la carta tenga una serie permitida en ese torneo
        public async Task<bool> RegistrarCartas(CrudMazoCartasDTO carta, int idTorneo, int userId)
        {
            //Chequeo que sea su mazo
            var mazo = await ControlarMazo(carta.IdMazo, userId);

            //Chequeo que la carta sea de su coleccion
            var cartaColeccionada = await ControlarCartaColeccionada(userId);

            //Controlo que el mazo en el que estoy intentando insertar, sea de ese jugador
            if (mazo != null && cartaColeccionada != null)
            {
                var cartas = await ControlarCartaSerie(carta.IdCarta, idTorneo);

                //si es 0, significa que la carta NO tiene una serie que acepte el torneo, si es mayor si se acepta la carta
                if (cartas.Count > 0)
                {
                    var cantidadCartas = await ContarCartasEnMazo(carta.IdMazo);
                    
                    if(cantidadCartas <= 15)
                    {
                        using var tran = _dbConnection.BeginTransaction();
                        //Inserto la carta en el MazoCarta correspondiente, controlar que no pueda inscribir mas de 15 cartas
                        var insert = await InsertarCartaEnMazo(carta.IdMazo, carta.IdCarta, tran);
                        
                        if (insert>0)
                        {
                            tran.Commit();
                            return true;
                        }
                        else
                        {
                            tran.Rollback();
                            return false;
                            throw new InvalidRequestException("Registro fallido. No fue posible inscribir la carta. Revise informacion");
                        }
                    }
                    else
                    { 
                        return false;
                        throw new InvalidRequestException("Registro fallido. No fue posible inscribir la carta debido a que el Mazo llego a su limite de 15 cartas");
                    }

                }
                else
                { 
                    return false; 
                    throw new InvalidRequestException("Registro fallido. No se pudo inscribir la carta porque tiene una serie que el torneo no acepta");
                }
            }
            else
                return false;
                throw new InvalidRequestException("Registro fallido. Ese Mazo no existe, o no es tuyo o la carta no es de tu coleccion");

        }

        public async Task<TorneoDTO> ControlarTorneo(int idTorneo)
        {
            var sqlTorneo = @"SELECT * FROM Torneos
                              WHERE TorneoID = @idTorneo;;";

            var resp = await _dbConnection.QuerySingleOrDefaultAsync<TorneoDTO>(sqlTorneo, new { idTorneo });

            return resp!;

        }

        public async Task<int> ContarInscriptosEnTorneo(int idTorneo)
        {
            var sqlContar = @"SELECT COUNT(IdJugador) from TorneoJugadores where IdTorneo=@idTorneo AND Aceptada=1;";

            var resp = await _dbConnection.ExecuteScalarAsync<int>(sqlContar, new { idTorneo });

            return resp;

        }


        public async Task<bool> RegistroEnTorneo(int idTorneo, int userId, int idMazo)
        {
            //Traigo info del torneo
            var torneo = await ControlarTorneo(idTorneo);

            //Veo si existe y su estado es Registro.
            if (torneo != null && torneo.Estado == "Registro")
            {
                //Controlar que el Mazo sea del Jugador y que tenga 15 cartas(Falta), y luego que sean de series que acepta el torneo
                var mazo = await ControlarMazo(idMazo, userId);

                var cantidadCartas = await ContarCartasEnMazo(idMazo);

                if (mazo != null && cantidadCartas == 15)
                {
                    //Contolamos la cantidad de inscriptos y que no supere el maxJugadores de torneo

                    var cantidadInscriptos = await ContarInscriptosEnTorneo(idTorneo);

                    if (cantidadInscriptos <= torneo.MaxJugadores)
                    {
                        using var tran = _dbConnection.BeginTransaction();
                        
                        //Lo inscribo en el torneo, con su respectivo Mazo
                        var sqlInsertar = @"INSERT
                                        INTO TorneoJugadores(IdTorneo, IdJugador, IdMazo, Aceptada) 
                                        VALUES(@TorneoID,@userId,@idMazo,1);";

                        var insert = await _dbConnection.ExecuteAsync(sqlInsertar, new { torneo.TorneoID, userId, idMazo }, transaction: tran);

                        if (insert>0)
                        {
                            tran.Commit();
                            return true;

                        }
                        else
                        {
                            tran.Rollback();
                            throw new InvalidRequestException("Registro fallido. No fue posible inscribir al jugador. Revise informacion");
                        }
                    }
                    else
                    {
                        return false;
                        throw new InvalidRequestException("Registro fallido. No fue posible inscribir al jugador debido a que no hay mas lugar");

                    }

                }
                else
                {
                    return false;
                    throw new InvalidRequestException("Registro fallido. No fue posible inscribir al jugador debido a que no es su mazo o este no tiene 15 cartas");
                }
            }
            return false;
            throw new InvalidRequestException("Registro fallido. No fue posible inscribir al jugador debido a que no existe el torneo o no esta en etapa de registro");
            
        }
    }
}
