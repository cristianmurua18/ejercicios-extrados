using Dapper;
using Entidades.DTOs;
using Entidades.DTOs.Cruds;
using Entidades.DTOs.Respuestas;
using Entidades.Modelos;
using System.Data;

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
            var sqlInsert = @"SELECT * from Mazos where JugadorCreador = @userId;";

            var mazo = await _dbConnection.QueryAsync<CrudMazoDTO>(sqlInsert, new { JugadorCreador = userId });

            return mazo.ToList();

        }



        //Creo una lista de cartas, IdMazo y IdCarta - A una mazo le asigno cierta cantidad de cartas
        //El proc debe pedir en que torneo sera utilizado el mazo y se debe chequear que la carta tenga una serie permitida en ese torneo
        public async Task<bool> RegistrarCartas(CrudMazoCartasDTO carta, int idTorneo, int userId)
        {
            //CREAR UN TARNSACCION 
            try
            {   //Controlo que el mazo en el que estoy intentando insertar, sea de ese jugador
                var sqlRevision = @"SELECT * from Mazos where MazoID = @IdMazo;";

                var mazo = await _dbConnection.QuerySingleOrDefaultAsync<CrudMazoDTO>(sqlRevision, new { carta.IdMazo });

                var sqlChequeo = @"SELECT IdCarta from CartasColeccionadas where IdUsuario = @userId;";

                var cartaColeccion = await _dbConnection.QuerySingleOrDefaultAsync<CartaColeccionDTO>(sqlChequeo, new { userId });

                if (mazo != null && cartaColeccion != null && mazo.JugadorCreador == userId)
                {
                    using (var tran = _dbConnection.BeginTransaction())
                    {
                        var sqlSelect = @"SELECT
                            c.CartaID,
	                        c.NombreCarta,
                            s.SerieID,
	                        s.NombreSerie
                            FROM CartaSerie cs
                            JOIN Cartas c ON cs.IdCarta = c.CartaID
                            JOIN Series s ON cs.IdSerie = s.SerieID
                            WHERE c.CartaID = @IdCarta AND
                            cs.IdSerie IN(SELECT IdSerie from TorneoSerieHabilitada WHERE IdTorneo = @idTorneo);";
                        //Seguir aqui
                        var cartas = await _dbConnection.QueryAsync<RespuestaCartaSerieDTO>(sqlSelect, new { carta.IdCarta, idTorneo }, transaction: tran);

                        //si es 0, significa que la carta no tiene una serie que acepte el torneo, si es mayor si se acepta la carta
                        if (cartas.Count() > 0)
                        {
                            //Inscribo la carta en el MazoCarta correspondiente
                            var sqlInsertar = @"INSERT
                                    INTO MazoCartas(IdMazo,IdCarta) 
                                    VALUES(@IdMazo,@IdCarta);";

                            var insert = await _dbConnection.ExecuteAsync(sqlInsertar, new { carta.IdMazo, carta.IdCarta }, transaction: tran);

                            if (insert>0)
                            {
                                tran.Commit();
                                return true;
                            }
                            else
                            {
                                tran.Rollback();
                                throw new InvalidOperationException("Registro fallido. No fue posible inscribir la carta. Revise informacion");
                            }
                        }
                        else
                            throw new InvalidOperationException("Registro fallido. No se pudo inscribir la carta porque tiene una serie que el torneo no acepta");
                    }
                }
                else
                    throw new InvalidOperationException("Registro fallido. Ese Mazo no existe, o no es tuyo o la carta no es tuya.");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }

        }

        public async Task<bool> RegistroEnTorneo(int idTorneo, int userId, int idMazo)
        {
            using (var tran = _dbConnection.BeginTransaction())
            {
                //Traigo info del torneo
                var sqlTorneo = @"SELECT * FROM Torneos
                    where TorneoID = @idTorneo;";

                var torneo = await _dbConnection.QuerySingleOrDefaultAsync<TorneoDTO>(sqlTorneo, new { idTorneo }, transaction: tran);
                //Veo si existe y su estado es Registro.
                if (torneo != null && torneo.Estado == "Registro")
                {
                    //Controlar que el Mazo sea del Jugador y que tenga 15 cartas(Falta), y luego que sean de series que acepta el torneo
                    var Sqlmazo = @"SELECT * FROM Mazos where JugadorCreador=@userId;";

                    var mazo = await _dbConnection.QuerySingleOrDefaultAsync<CrudMazoDTO>(Sqlmazo, new { userId }, tran);

                    if (mazo != null && mazo.MazoID == idMazo)
                    {
                        //Contolamos la cantidad de inscriptos y que no supere el maxJugadores de torneo

                        var contarInscriptos = "SELECT COUNT(IdJugador) from TorneoJugadores where IdTorneo=@idTorneo AND Aceptada=1;";
                        //SIN , transaction: tran. Hay error?
                        var cantidad = await _dbConnection.ExecuteScalarAsync<int>(contarInscriptos, new { idTorneo }, tran);

                        if (cantidad <= torneo.MaxJugadores)
                        {
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
                                throw new InvalidOperationException("Registro fallido. No fue posible inscribir al jugador. Revise informacion");
                            }

                        }
                        else
                        {
                            tran.Rollback();
                            throw new InvalidOperationException("Registro fallido. No fue posible inscribir al jugador debido a que no hay mas lugar");

                        }

                    
                    }
                    else
                    {
                        tran.Rollback();
                        throw new InvalidOperationException("Registro fallido. No fue posible inscribir al jugador debido a que no es su mazo");
                    }

                     
                }
                return false;
                throw new InvalidOperationException("Registro fallido. No fue posible inscribir al jugador debido a que no existe el torneo o ya no esta en etapa de registro");
            }
        }

    }
}
