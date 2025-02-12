using Dapper;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entidades.DTOs.Jugadores;

namespace AccesoDatos.DAOs
{
    public class DAOJuez(IDbConnection dbConnection) : IDAOJuez
    {

        private readonly IDbConnection _dbConnection = dbConnection;


        #region Uso general de Jueces

        public async Task<bool> OficializarResultados()
        {       //Revisar funcionamiento
            var sqlInsert = @"INSERT INTO JugadorDescalificaciones(Motivo,JuezDescalificador,JugadorID) 
                                     VALUES(@Motivo,@JuezDescalificador,@JugadorID);";

            var resultado = await _dbConnection.ExecuteAsync(sqlInsert);

            return resultado > 0;

        }

        public async Task<bool> DescalificarJugador(JugadorDescalificadoDTO jugadorDescalificado)
        {
            var sqlInsert = @"INSERT INTO JugadorDescalificaciones(Motivo,JuezDescalificador,JugadorID) 
                                     VALUES(@Motivo,@JuezDescalificador,@JugadorID);";

            var resultado = await _dbConnection.ExecuteAsync(sqlInsert, jugadorDescalificado);

            return resultado > 0;

        }



        #endregion
    }
}
