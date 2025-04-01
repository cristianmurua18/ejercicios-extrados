
using Dapper;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entidades.DTOs.Jugadores;
using Entidades.Modelos;
using Entidades.DTOs.Respuestas;
using Entidades.DTOs;

namespace AccesoDatos.DAOs.Juez
{
    public class DAOJuez(IDbConnection dbConnection) : IDAOJuez
    {

        private readonly IDbConnection _dbConnection = dbConnection;


        #region Uso general de Jueces

        public async Task<TorneoDTO> VerTorneo(int idTorneo)
        {
            var sqlSelect = @"SELECT * FROM Torneos
                              WHERE TorneoID=@idTorneo;";

            var respuesta = await _dbConnection.QuerySingleOrDefaultAsync<TorneoDTO>(sqlSelect, new { idTorneo });

            return respuesta;

        }
        public async Task<TorneoJuecesDTO> CorroborarJuezYTorneo(int idTorneo, int idUsuario)
        {
            var sqlSelect = @"SELECT * FROM TorneoJueces
                              WHERE IdTorneo=@idTorneo AND IdJuez=@idUsuario;";

            var respuesta = await _dbConnection.QuerySingleOrDefaultAsync<TorneoJuecesDTO>(sqlSelect, new { idTorneo, idUsuario });

            return respuesta;

        }


        public async Task<List<PartidaRondaDTO>> VerRondasYPartidas(int idTorneo)
        {       
            var sqlSelect = @"SELECT * FROM Partidas par
                              INNER JOIN Rondas ron
                              ON par.IdRonda = ron.RondaID
                              WHERE ron.IdTorneo=@idTorneo;";

            var listado = await _dbConnection.QueryAsync<PartidaRondaDTO>(sqlSelect, new { idTorneo} );

            return listado.ToList();

        }

        public async Task<List<PartidaRondaDTO>> VerRondasYPartidas(int idTorneo, int idRonda)
        {
            var sqlSelect = @"SELECT * FROM Partidas par
                              INNER JOIN Rondas ron
                              ON par.IdRonda = ron.RondaID
                              WHERE ron.IdTorneo=@IdTorneo AND ron.RondaID=@IdRonda AND par.IdRonda=@IdRonda;";

            var listado = await _dbConnection.QueryAsync<PartidaRondaDTO>(sqlSelect, new { idTorneo, idRonda });

            return listado.ToList();

        }

        public async Task<bool> OficializarResultadoEnPartida(int idTorneo, int idPartida, int idGanador)
        {       //Revisar funcionamiento
            //IDEA: Primero revisar torneo, y obtener los datos de los jugadores y que no meta cualquier cosa.
            var sqlInsert = @"UPDATE Partidas
                              SET Ganador=@idGanador
                              WHERE PartidaID=@idPartida;";

            var resultado = await _dbConnection.ExecuteAsync(sqlInsert, new { idGanador, idPartida });

            return resultado > 0;

        }
        public async Task<bool> OficializarResultadoEnTorneo(int idGanador, int idTorneo)
        {    
            var sqlInsert = @"UPDATE Torneos
                              SET JugadorGanador=@idGanador
                              WHERE TorneoID=@idTorneo;";

            var resultado = await _dbConnection.ExecuteAsync(sqlInsert, new { idGanador, idTorneo });

            return resultado > 0;

        }
        public async Task<bool> DescalificarJugador(JugadorDescalificadoDTO jugadorDescalificado)
        {
            var sqlInsert = @"INSERT INTO JugadorDescalificaciones(Motivo,IdTorneo, JuezDescalificador,JugadorID) 
                                     VALUES(@Motivo, @IdTorneo, @JuezDescalificador,@JugadorID);";

            var resultado = await _dbConnection.ExecuteAsync(sqlInsert, jugadorDescalificado);

            return resultado > 0;

        }



        #endregion
    }
}
