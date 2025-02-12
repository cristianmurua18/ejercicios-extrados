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

namespace AccesoDatos.DAOs
{
    public class DAOJugador(IDbConnection dbConnection) : IDAOJugador
    {

        private readonly IDbConnection _dbConnection = dbConnection;

        //Creo un Mazo, le pongo nombre y jugador que lo creo
        public async Task<int> CrearMazo(CrudMazoDTO mazo)
        {
            var sqlInsert = @"INSERT
                INTO Mazos(Nombre,JugadorCreador)
                OUTPUT INSERTED.MazoID
                VALUES(@Nombre,@JugadorCreador);"
            ;
            
            var mazoID = await _dbConnection.ExecuteScalarAsync<int>(sqlInsert, mazo);

            return mazoID;

        }
        
        //Creo una lista de cartas, IdMazo y IdCarta - A una mazo le asigno cierta cantidad de cartas
        //Esta cantidad de cartas no puede superar 15 - VER CHECK
        public async Task<bool> RegistrarCartas(CrudMazoCartasDTO cartas)
        {
            var response = "";

            var trans = _dbConnection.BeginTransaction();

            var sqlInsert = @"INSERT
                INTO MazoCartas(IdMazo, IdCarta) 
                VALUES(@IdMazo,@IdCarta);"; //Revisar validacion si es correcta
                
            var result1 = await _dbConnection.ExecuteAsync(sqlInsert, cartas);

            if (result1 == 1)
                response = "OK";
            else
                response = "Error en el ingreso de datos";

            if (response.Equals("OK"))
            {
                var sqlSelect = @"SELECT COUNT(IdCarta) AS Cantidad from MazoCartas;";

                var result2 = await _dbConnection.ExecuteScalarAsync<int>(sqlSelect);

                if (result2 <= 15)
                {
                    trans.Commit();
                }
            }
            else
                trans.Rollback();
            
            return result1 > 0;

        }


    }
}
