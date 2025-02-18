using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Entidades.Modelos;
using Entidades.DTOs.Cruds;
using Entidades.DTOs.Respuestas;

namespace AccesoDatos.DAOs.Jugador
{
    public class DAOJugador(IDbConnection dbConnection) : IDAOJugador
    {

        private readonly IDbConnection _dbConnection = dbConnection;

        //Creo un Mazo, le pongo nombre y jugador que lo creo
        public async Task<int> CrearMazo(int userId, string nombreMazo)
        {
            var sqlInsert = @"INSERT
                INTO Mazos(NombreMazo,JugadorCreador)
                OUTPUT INSERTED.MazoID
                VALUES(@NombreMazo,@JugadorCreador);"
            ;
            
            var mazoID = await _dbConnection.ExecuteScalarAsync<int>(sqlInsert, new { NombreMazo = nombreMazo, JugadorCreador = userId });

            return mazoID;

        }
        
        //Creo una lista de cartas, IdMazo y IdCarta - A una mazo le asigno cierta cantidad de cartas
        //Esta cantidad de cartas no puede superar 15 - VER CHECK. OKA
        //El proc debe pedir en que torneo sera utilizado el mazo y se debe chequear que la carta tenga una serie permitida en ese torneo
        public async Task<dynamic> RegistrarCartas(CrudMazoCartasDTO cartas, int torneoID)
        {
            var carta = await _dbConnection.QueryAsync(
                sql: "dbo.RevisarCantCartas",
                param: new { cartas.IdMazo, cartas.IdCarta, TorneoID = torneoID},
                commandType: CommandType.StoredProcedure);
         
            return carta;

        }

        





    }
}
