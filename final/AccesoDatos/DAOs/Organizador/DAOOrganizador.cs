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

        public async Task<int> ContarInscriptosByTorneo(int idTorneo)
        {
            var sql = "SELECT COUNT(IdJugador) FROM InfoJugadorTorneo WHERE IdTorneo=idTorneo;";

            var cantidad = await _dbConnection.ExecuteScalarAsync<int>(sql, new { idTorneo});

            return cantidad;

        }

        public async Task<int> ContarInscriptosByTorneo(int idTorneo, IDbTransaction transaction)
        {
            var sql = "SELECT COUNT(IdJugador) FROM InfoJugadorTorneo WHERE IdTorneo=idTorneo;";

            var cantidad = await _dbConnection.ExecuteScalarAsync<int>(sql, new { idTorneo }, transaction);

            return cantidad;

        }


        //Para Calcular Cantidad de partidas, organizador viene de id del usuario autenticado
        public async Task<TorneoDTO> TraerTorneo(int organizador, int idTorneo)
        {
            var sqlSelect = @"SELECT * from Torneos WHERE Organizador=@organizador AND TorneoID=@idTorneo;";

            var torneo = await _dbConnection.QueryFirstOrDefaultAsync<TorneoDTO>(sqlSelect, new { organizador, idTorneo});

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
            var listado = await _dbConnection.QueryAsync<Usuario>(sqlSelect, new { Rol = rol});

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

            var resultado = await _dbConnection.ExecuteAsync(sqlInsert, new { serie.IdTorneo, serie.IdSerie});

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

        public async Task GenerarRondasYPartidas(int organizador, int idTorneo)
        {
            using var tran = _dbConnection.BeginTransaction();

            int rondaID = 1;
            //En realidad son todos los jugadores del torneo
            int jugadoresRestantes = await ContarInscriptosByTorneo(idTorneo, tran);

            if (jugadoresRestantes % 2 == 1)
                throw new InvalidOperationException("No es posible iniciar un torneo con cantidad impar de jugadores");


            var torneo = await TraerTorneo(organizador, idTorneo, tran);

            if (torneo!=null)
            {
                while (jugadoresRestantes > 1)
                {
                    int cantidadPartidas = jugadoresRestantes / 2;
                    // Insertar la ronda
                    string sqlInsertRonda = "INSERT INTO Rondas (RondaID, IdTorneo, CantidadPartidas) VALUES (@RondaID, @IdTorneo, @CantidadPartidas)";
                    await _dbConnection.ExecuteAsync(sqlInsertRonda, new { RondaID = rondaID, IdTorneo = torneo.TorneoID, CantidadPartidas = cantidadPartidas }, transaction: tran);

                    // Insertar las partidas
                    for (int i = 1; i <= cantidadPartidas; i++)
                    {
                        string sqlInsertPartida = "INSERT INTO Partidas (PartidaID, IdRonda, FyHInicioP) VALUES (@PartidaID, @IdRonda, @FyHInicioP)";
                        await _dbConnection.ExecuteAsync(sqlInsertPartida, new { PartidaID = i, IdRonda = rondaID, FyHInicioP = torneo.FyHInicioT }, transaction: tran);
                        torneo.FyHInicioT = torneo.FyHInicioT.AddMinutes(30);
                    }

                    // Avanzar a la siguiente ronda
                    jugadoresRestantes = (jugadoresRestantes % 2 == 0) ? jugadoresRestantes / 2 : (jugadoresRestantes / 2) + 1;
                    rondaID++;
                }
                tran.Commit();
            }
            else
                tran.Rollback();
                throw new InvalidOperationException("Registro fallido. No fue posible ubicar el torneo.Revise informacion.");
        }


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
