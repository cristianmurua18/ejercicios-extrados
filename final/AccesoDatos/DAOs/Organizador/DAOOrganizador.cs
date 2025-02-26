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


        //Para Calcular Cantidad de partidas, organizador viene de id del usuario autenticado
        public async Task<TorneoDTO> TraerTorneo(int organizador, int idTorneo)
        {
            var sqlSelect = @"SELECT * from Torneos WHERE Organizador=@organizador AND TorneoID=@idTorneo;";

            var torneo = await _dbConnection.QueryFirstOrDefaultAsync<TorneoDTO>(sqlSelect, new { organizador, idTorneo});

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
