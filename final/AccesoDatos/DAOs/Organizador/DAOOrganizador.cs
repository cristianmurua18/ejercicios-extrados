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
using Entidades.DTOs.Respuestas;

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
            var sql = "UPDATE TorneoJugadores SET Aceptada=0 WHERE IdJugador=idJugador;";

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

        public async Task<bool> AsignarJuezATorneo(int idJuez, int idTorneo)
        {
            var sqlInsert = @"INSERT
                INTO TorneoJueces 
                VALUES(@IdJuez,@IdTorneo);";

            var result = await _dbConnection.ExecuteAsync(sqlInsert, new { idJuez, idTorneo});

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
            var sqUpdate =
                @"UPDATE Torneos 
                SET Estado=@estado 
                WHERE TorneoID=@idtorneo;";
            //Pongo directamente cancelado?
            var resultado = await _dbConnection.ExecuteAsync(sqUpdate, new { idtorneo });

            return resultado > 0;

        }

        public async Task<bool> CerrarInscrpcionTorneo(int idTorneo)
        {
            var sqUpdate =
                @"UPDATE Torneos 
                SET Estado='Partidas' 
                WHERE TorneoID=@idtorneo;";
            
            var resultado = await _dbConnection.ExecuteAsync(sqUpdate, new { idTorneo });

            return resultado > 0;
        }
        public async Task<bool> GenerarRondasYPartidas(int organizador, int idTorneo)
        {
            using var tran = _dbConnection.BeginTransaction();

            //Traigo el torneo organizado por mi
            var torneo = await TraerTorneo(organizador, idTorneo, tran);

            //Cuento la cantidad de inscriptos
            var inscriptos = await ContarInscriptosByTorneo(idTorneo, tran);

            // Validar bit a bit que la cantidad de jugadores sea potencia de 2
            if ((inscriptos & (inscriptos - 1)) != 0)
                throw new InvalidOperationException("La cantidad de jugadores debe ser una potencia de 2. Revise inscriptos. Operacion cancelada.");

            //Traigo los jugadores inscriptos, un listado de sus ids
            var sqlInscriptos = "SELECT IdJugador from TorneoJugadores where IdTorneo=@idTorneo";

            var verInscriptos = await _dbConnection.QueryAsync<TorneoJugadorDTO>(sqlInscriptos, new { IdTorneo = idTorneo }, tran);
            //los pongo en una pila, para luego asignarlos en la creacion de partidas
            var pilaJugadores = new Stack<int>(verInscriptos.Select(jug => jug.IdJugador));

            //Valido que exista el torneo y seas el organizador, y que esten en estado partidas. Con las incripciones cerradas
            if (torneo!=null && (torneo.Estado == "Partidas"))
            {

                var partidasTotales = inscriptos - 1;

                int cantidadRondas = (int)Math.Log2(inscriptos);
                int partidasEnRonda = inscriptos / 2;

                int? jugadorUno = null;
                int? jugadorDos = null;

                // Insertar rondas
                for (int i = 1; i <= cantidadRondas; i++)
                {
                    var idRonda = await _dbConnection.ExecuteScalarAsync<int>(
                        @"INSERT INTO Rondas 
                        OUTPUT INSERTED.RondaID
                        VALUES (@IdTorneo, @NumeroRonda);",
                        new { IdTorneo = idTorneo, NumeroRonda = i}, tran);

                    if (!(idRonda > 0))
                    {
                        tran.Rollback();
                        return false;
                    }

                    //Insertar partidas
                    for (int j = 1; j <= partidasEnRonda; j++)
                    {
                        if (pilaJugadores.Count < 2)
                        {
                            tran.Rollback();
                            throw new InvalidOperationException("No hay suficientes jugadores para generar las partidas.");
                        }

                        if(i == 1) //Esto es todas las partidas que se creen en la primera ronda?
                        {
                            jugadorUno = pilaJugadores.Pop();
                            jugadorDos = pilaJugadores.Pop();
                        }

                        await _dbConnection.ExecuteAsync(
                            @"INSERT INTO Partidas (IdRonda, NumeroPartida, FyHInicioP, JugadorUno, JugadorDos)
                            VALUES (@IdRonda, @NumeroPartida, @FyHInicioP, @JugadorUno, @JugadorDos);",
                            new { IdRonda = idRonda, NumeroPartida=j, FyHInicioP = torneo.FyHInicioT, JugadorUno = jugadorUno, JugadorDos = jugadorDos }, tran);

                        torneo.FyHInicioT = torneo.FyHInicioT.AddMinutes(30);
                        //Aca la idea del torneo es jugar hasta las 18hs del dia, asi que debo controlar que si la hora es igual a las 17.30, debo agregar un dia y comenzar a las 10hs
                        if (torneo.FyHInicioT.Hour == 17 && torneo.FyHInicioT.Minute >= 30)
                        {
                            torneo.FyHInicioT = torneo.FyHInicioT.Date.AddDays(1).AddHours(10);
                        }


                    }
                    partidasEnRonda /= 2; // La siguiente ronda tiene la mitad de partidas
                }
                tran.Commit();
                return true;
            }
            return false;
            throw new ArgumentException("No existe el torneo o tu no lo organizas, luego debes cerrar inscripciones. Operacion cancelada.");
        }

        public async Task<bool> ModificarPartida(PartidaDTO partida)
        {
            //Revisar 
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
