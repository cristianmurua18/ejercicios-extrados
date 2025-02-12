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

namespace AccesoDatos.DAOs
{
    public class DAOOrganizador(IDbConnection dbConnection) : IDAOOrganizador
    {

        private readonly IDbConnection _dbConnection = dbConnection;


        #region Uso general de Organizadores


        public async Task<bool> RegistrarJuez(CrudUsuarioDTO usuario)
        {
            var sqlInsert = @"INSERT
                INTO Usuarios(NombreApellido,Alias,IdPais,Email,NombreUsuario,Contraseña,FotoAvatar,Rol,CreadorPor,Activo) 
                VALUES(@NombreApellido,@Alias,@IdPais,@Email,@NombreUsuario,@Contraseña,@FotoAvatar,@Rol,@CreadoPor,@Activo);";

            var result = await _dbConnection.ExecuteAsync(sqlInsert, usuario);

            return result > 0;

        }



        public async Task<bool> CrearTorneo(CrudTorneoDTO torneo)
        {
            var sqlInsert = @"INSERT INTO Torneos(FyHInicio,FyHFin,Pais,Fase,RondaActual,JugadorGanador,Organizador) 
                                     VALUES(@FyHInicio,@FyHFin,@Pais,@Fase,@RondaActual,@JugadorGanador,@Organizador);";

            var resultado = await _dbConnection.ExecuteAsync(sqlInsert, torneo);

            //Agrega un torneo con Exito?
            return resultado > 0;
        }

        public async Task<bool> EditarTorneo(CrudTorneoDTO torneo)
        {
            var sqlUpdate =
                @"UPDATE Torneos 
                SET FyHInicio=@FyHInicio, FyHFin=@FyHFin, Pais=@Pais, Fase=@Fase, RondaActual=@RondaActual, 
                JugadorGanador=@JugadorGanador, Organizador=@Organizador 
                WHERE TorneoID=@TorneoID;";

            var resultado = await _dbConnection.ExecuteAsync(sqlUpdate, torneo);

            return resultado > 0;

        }

        public async Task<bool> CancelarTorneo(int idtorneo, string estado)
        {
            var sqlDelete =
                @"UPDATE Torneos 
                SET Estado=@estado
                WHERE TorneoID=@idtorneo;";            ;

            var resultado = await _dbConnection.ExecuteAsync(sqlDelete, new { idtorneo });

            return resultado > 0;

        }

        public async Task<bool> CrearPartida(PartidaDTO partida)
        {
            var sqlInsert = @"INSERT INTO Partidas(FyHInicio,FyHFin,HsDiarias,Ronda,JugadorUno,JugadorDos,JugadorVencedor) 
                                     VALUES(@FyHInicio,@FyHFin,@HsDiarias,@Ronda,@JugadorUno,@JugadorDos,@JugadorVencedor);";

            var resultado = await _dbConnection.ExecuteAsync(sqlInsert, partida);

            //Agrega un torneo con Exito?
            return resultado > 0;

        }

        public async Task<bool> ModificarPartida(PartidaDTO partida)
        {
            var sqlUpdate =
                @"UPDATE Partidas 
                SET FyHInicio=@FyHInicio, FyHFin=@FyHFin, HsDiarias=@HsDiarias, Ronda=@Ronda, JugadorUno=@JugadorUno, 
                JugadorDos=@JugadorDos, JugadorVencedor=@JugadorVencedor 
                WHERE PartidaID=@PartidaID;";

            var resultado = await _dbConnection.ExecuteAsync(sqlUpdate, partida);

            return resultado > 0;
        }


        #endregion
    }
}
