using Entidades.DTOs;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entidades.Modelos;
using Entidades.DTOs.Cruds;
using System.Collections;

namespace AccesoDatos.DAOs.Organizador
{
    public class DAOOrganizador(IDbConnection dbConnection) : IDAOOrganizador
    {

        private readonly IDbConnection _dbConnection = dbConnection;


        #region Uso general de Organizadores

        public async Task<List<TorneoJugadorDTO>> VerInscriptosByTorneo(int idTorneo)
        {
            var sql = "SELECT * FROM TorneoJugadores WHERE IdTorneo=idTorneo;";

            var inscriptos = await _dbConnection.QueryAsync<TorneoJugadorDTO>(sql, new { idTorneo });

            return inscriptos.ToList();

        }
        public async Task<bool> EliminarInscriptoByTorneo(int idJugador)
        {                                                                   //Agregar condicion Aceptada=0
            var sql = "DELETE FROM TorneoJugadores WHERE IdJugador=idJugador;";

            var filas = await _dbConnection.ExecuteAsync(sql, new { idJugador });

            return filas > 0;

        }
        public async Task<int> ContarInscriptosByTorneo(int idTorneo)
        {
            var sql = "SELECT COUNT(IdJugador) FROM TorneoJugadores WHERE IdTorneo=idTorneo AND Aceptada=1;";

            var cantidad = await _dbConnection.ExecuteScalarAsync<int>(sql, new { idTorneo });

            return cantidad;

        }


        public async Task<int> ContarInscriptosByTorneo(int idTorneo, IDbTransaction transaction)
        {
            var sql = "SELECT COUNT(IdJugador) FROM TorneoJugadores WHERE IdTorneo=idTorneo AND Aceptada=1;";

            var cantidad = await _dbConnection.ExecuteScalarAsync<int>(sql, new { idTorneo }, transaction);

            return cantidad;

        }


        //Para Calcular Cantidad de partidas, organizador viene de id del usuario autenticado
        public async Task<TorneoDTO> TraerTorneo(int organizador, int idTorneo)
        {
            var sqlSelect = @"SELECT * from Torneos WHERE Organizador=@organizador AND TorneoID=@idTorneo;";

            var torneo = await _dbConnection.QueryFirstOrDefaultAsync<TorneoDTO>(sqlSelect, new { organizador, idTorneo });

            return torneo!;

        }

        public async Task<TorneoDTO> TraerTorneo(int organizador, int idTorneo, IDbTransaction transaction)
        {
            var sqlSelect = @"SELECT * from Torneos WHERE Organizador=@organizador AND TorneoID=@idTorneo;";

            var torneo = await _dbConnection.QueryFirstOrDefaultAsync<TorneoDTO>(sqlSelect, new { organizador, idTorneo }, transaction);

            return torneo!;

        }


        public async Task<List<Usuario>> VerListadoUsuarios(string rol)
        {                                           //ver de manejar estado 1 o 0
            var sqlSelect = @"SELECT *u2.NombreApellido, u2.Alias, u2.IdPaisOrigen, u2.Email, 
                            u2.NombreUsuario, u2.FotoAvatar, u2.Rol, u2.Activo
                            FROM usuarios u1
                            JOIN usuarios u2 ON u1.UsuarioID = u2.CreadoPor
                            WHERE Rol=@rol;";
            //SEGUIR REVISANDO, NO FUNCIONA BIEN
            var listado = await _dbConnection.QueryAsync<Usuario>(sqlSelect, new { Rol = rol });

            return [.. listado];

        }


        public async Task<bool> RegistrarJuez(CrudUsuarioDTO usuario)
        {
            var sqlInsert = @"INSERT
                INTO Usuarios(NombreApellido,Alias,IdPaisOrigen,Email,NombreUsuario,Contraseña,FotoAvatar,Rol,CreadoPor,Activo) 
                VALUES(@NombreApellido,@Alias,@IdPaisOrigen,@Email,@NombreUsuario,@Contraseña,@FotoAvatar,@Rol,@CreadoPor,@Activo);";

            var result = await _dbConnection.ExecuteAsync(sqlInsert, usuario);

            return result > 0;

        }


        public async Task<bool> CrearTorneo(CrudTorneoDTO torneo)
        {
            var sqlInsert = @"INSERT INTO Torneos(FyHInicioT,FyHFinT,Estado,IdPaisRealizacion,PartidaActual,JugadorGanador,Organizador, PartidasDiarias, DiasDeDuracion) 
                                     VALUES(@FyHInicioT,@FyHFinT,@Estado,@IdPaisRealizacion,@PartidaActual,@JugadorGanador,@Organizador,@PartidasDiarias,@DiasDeDuracion);";

            var resultado = await _dbConnection.ExecuteAsync(sqlInsert, torneo);

            //Agrega un torneo con Exito?
            return resultado > 0;
        }

        public async Task<bool> CrearTorneoSerieHabilitada(CrudTorneoSerieHabilitadaDTO serie)
        {
            var sqlInsert = @"INSERT INTO TorneoSerieHabilitada(IdTorneo,IdSerie) 
                                     VALUES(@IdTorneo,@IdSerie);";

            var resultado = await _dbConnection.ExecuteAsync(sqlInsert, new { serie.IdTorneo, serie.IdSerie });

            //Agrega un torneo con Exito?
            return resultado > 0;
        }


        public async Task<bool> EditarTorneo(CrudTorneoDTO torneo)
        {
            var sqlUpdate =
                @"UPDATE Torneos 
                SET FyHInicioT=@FyHInicioT, FyHFinT=@FyHFinT, Estado=@Estado, IdPaisRealizacion=@IdPaisRealizacion, PartidaActual=@PartidaActual, 
                JugadorGanador=@JugadorGanador, Organizador=@Organizador, PartidasDiarias=@PartidasDiarias,DiasDeDuracion=@DiasDeDuracion
                WHERE TorneoID=@TorneoID;";

            var resultado = await _dbConnection.ExecuteAsync(sqlUpdate, torneo);

            return resultado > 0;

        }

        public async Task<bool> CancelarTorneo(int idtorneo, string estado)
        {
            var sqlDelete =
                @"UPDATE Torneos 
                SET Estado=@estado 
                WHERE TorneoID=@idtorneo;";
            //Pongo directamente cancelado?
            var resultado = await _dbConnection.ExecuteAsync(sqlDelete, new { idtorneo });

            return resultado > 0;

        }

        public async Task<bool> GenerarRondasYPartidas(int organizador, int idTorneo)
        {
            using var tran = _dbConnection.BeginTransaction();

            //Traigo el torneo organizado por mi
            var torneo = await TraerTorneo(organizador, idTorneo, tran);

            //Cuento la cantidad de inscriptos
            var inscriptos = await ContarInscriptosByTorneo(idTorneo, tran);

            //Traigo los jugadores inscriptos, un listado 
            var sqlInscriptos = "SELECT IdJugador from TorneoInscriptos where IdTorneo=@idTorneo";

            var verInscriptos = await _dbConnection.QueryAsync<TorneoJugadorDTO>(sqlInscriptos, new { IdTorneo = idTorneo });
            //los pongo en una pila, para luego asignarlos en la creacion de partidas
            var pilaJugadores = new Stack<int>(verInscriptos.Select(jug => jug.IdJugador));

            //var inscriptos = 2;
                        //Revisar cuales serian las mejores validaciones
            if (torneo!=null && (torneo.Estado != "Cancelado" || torneo.Estado != "Finalizado"))
            {

                var partidasTotales = inscriptos - 1;

                // Validar bit a bit que la cantidad de jugadores sea potencia de 2
                if ((inscriptos & (inscriptos - 1)) != 0)
                    throw new ArgumentException("La cantidad de jugadores debe ser una potencia de 2. Revise inscriptos. Operacion cancelada.");

                int cantidadRondas = (int)Math.Log2(inscriptos);
                int partidasEnRonda = inscriptos / 2;
                var rondas = new List<int>();

                // Insertar rondas, los numeros qudan guardados en un array de enteros. 1, 2, 3,4 ,5 ,6 ,etc
                for (int i = 1; i <= cantidadRondas; i++)
                {
                    var affectedrows = await _dbConnection.ExecuteAsync(
                        "INSERT INTO Rondas VALUES (@IdRonda, @IdTorneo)",
                        new { IdRonda = i, IdTorneo = idTorneo });
                    rondas.Add(i);

                    if (affectedrows == 0)
                    {
                        tran.Rollback();
                        return false;
                    }
                }


                // Insertar partidas
                for (int i = 0; i < cantidadRondas; i++)
                {
                    for (int j = 0; j < partidasEnRonda; j++)
                    {
                        if (pilaJugadores.Count < 2)
                        {
                            tran.Rollback();
                            throw new InvalidOperationException("No hay suficientes jugadores para generar las partidas.");
                        }

                        int jugadorUno = pilaJugadores.Pop();
                        int jugadorDos = pilaJugadores.Pop();

                        await _dbConnection.ExecuteAsync(
                            "INSERT INTO Partidas (IdRonda, FyHInicioP, JugadorUno, JugadorDos, Ganador) VALUES (@IdRonda, @FyHInicioP, @JugadorUno, @JugadorDos)",
                            new { IdRonda = rondas[i], FyHInicioP = torneo.FyHInicioT, JugadorUno = jugadorUno, JugadorDos = jugadorDos });

                        torneo.FyHInicioT = torneo.FyHInicioT.AddMinutes(30);
                        //Aca la idea del torneo es jugar hasta las 18hs del dia, asi que debo controlar que si la hora es igual a las 17.30, debo agregar un dia y comenzar a las 10hs
                        if (torneo.FyHInicioT.Hour == 17 && torneo.FyHInicioT.Minute >= 30)
                        {
                            torneo.FyHInicioT = torneo.FyHInicioT.Date.AddDays(1).AddHours(10);
                        }


                    }
                    partidasEnRonda /= 2; // La siguiente ronda tiene la mitad de partidas
                }
                return true;
            }
            return false;
            throw new ArgumentException("No existe el torneo o tu no lo organizas. Operacion cancelada.");
        }

        //public async Task GenerarRondasYPartidas(int organizador, int idTorneo)
        //{
        //    using var tran = _dbConnection.BeginTransaction();

        //    int rondaID = 1;
        //    //En realidad son todos los jugadores del torneo
        //    int jugadoresRestantes = await ContarInscriptosByTorneo(idTorneo, tran);

        //    if (jugadoresRestantes % 2 == 1)
        //        throw new InvalidOperationException("No es posible iniciar un torneo con cantidad impar de jugadores");


        //    var torneo = await TraerTorneo(organizador, idTorneo, tran);

        //    if (torneo!=null)
        //    {
        //        while (jugadoresRestantes > 1)
        //        {
        //            int cantidadPartidas = jugadoresRestantes / 2;
        //            // Insertar la ronda
        //            string sqlInsertRonda = "INSERT INTO Rondas (RondaID, IdTorneo, CantidadPartidas) VALUES (@RondaID, @IdTorneo, @CantidadPartidas)";
        //            await _dbConnection.ExecuteAsync(sqlInsertRonda, new { RondaID = rondaID, IdTorneo = torneo.TorneoID, CantidadPartidas = cantidadPartidas }, transaction: tran);

        //            // Insertar las partidas
        //            for (int i = 1; i <= cantidadPartidas; i++)
        //            {
        //                string sqlInsertPartida = "INSERT INTO Partidas (PartidaID, IdRonda, FyHInicioP) VALUES (@PartidaID, @IdRonda, @FyHInicioP)";
        //                await _dbConnection.ExecuteAsync(sqlInsertPartida, new { PartidaID = i, IdRonda = rondaID, FyHInicioP = torneo.FyHInicioT }, transaction: tran);
        //                torneo.FyHInicioT = torneo.FyHInicioT.AddMinutes(30);
        //            }

        //            // Avanzar a la siguiente ronda
        //            jugadoresRestantes = (jugadoresRestantes % 2 == 0) ? jugadoresRestantes / 2 : (jugadoresRestantes / 2) + 1;
        //            rondaID++;
        //        }
        //        tran.Commit();
        //    }
        //    else
        //        tran.Rollback();
        //        throw new InvalidOperationException("Registro fallido. No fue posible ubicar el torneo.Revise informacion.");
        //}


        public async Task<bool> CrearRondas(RondaDTO ronda)
        {
            var sqlInsert = @"INSERT INTO Rondas(RondaID, IdTorneo,CantidadPartidas,JugadoresPar) 
                                     VALUES(@RondaID, @IdTorneo, @CantidadPartidas, @JugadoresPar);";

            var resultado = await _dbConnection.ExecuteAsync(sqlInsert, ronda);

            //Agrega un torneo con Exito?
            return resultado > 0;

        }
        public async Task<bool> CrearPartida(PartidaDTO partida)
        {
            var sqlInsert = @"INSERT INTO Partidas(FyHInicioP,FyHFinP,Ronda,JugadorDerrotado,JugadorVencedor) 
                                     VALUES(@FyHInicioP,@FyHFinP,@Ronda,@JugadorDerrotado,@JugadorVencedor);";

            var resultado = await _dbConnection.ExecuteAsync(sqlInsert, partida);

            //Agrega un torneo con Exito?
            return resultado > 0;

        }

        public async Task<bool> ModificarPartida(PartidaDTO partida)
        {
            var sqlUpdate =
                @"UPDATE Partidas 
                SET FyHInicioP=@FyHInicioP, FyHFinP=@FyHFinP, Ronda=@Ronda, JugadorDerrotado=@JugadorDerrotado, 
                JugadorVencedor=@JugadorVencedor WHERE PartidaID=@PartidaID;";

            var resultado = await _dbConnection.ExecuteAsync(sqlUpdate, partida);

            return resultado > 0;
        }






        #endregion
    }
}
