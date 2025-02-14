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

namespace AccesoDatos.DAOs.Organizador
{
    public class DAOOrganizador(IDbConnection dbConnection) : IDAOOrganizador
    {

        private readonly IDbConnection _dbConnection = dbConnection;


        #region Uso general de Organizadores


        public async Task<bool> RegistrarJuez(CrudUsuarioDTO usuario)
        {
            var sqlInsert = @"INSERT
                INTO Usuarios(NombreApellido,Alias,IdPaisOrigen,Email,NombreUsuario,Contraseña,FotoAvatar,Rol,CreadorPor,Activo) 
                VALUES(@NombreApellido,@Alias,@IdPaisOrigen,@Email,@NombreUsuario,@Contraseña,@FotoAvatar,@Rol,@CreadoPor,@Activo);";

            var result = await _dbConnection.ExecuteAsync(sqlInsert, usuario);

            return result > 0;

        }



        public async Task<bool> CrearTorneo(CrudTorneoDTO torneo)
        {
            var sqlInsert = @"INSERT INTO Torneos(FyHInicioT,FyHFinT,Estado,IdPaisRealizacion,PartidaActual,JugadorGanador,Organizador) 
                                     VALUES(@FyHInicioT,@FyHFinT,@Estado,@IdPaisRealizacion,@PartidaActual,@JugadorGanador,@Organizador);";

            var resultado = await _dbConnection.ExecuteAsync(sqlInsert, torneo);

            //Agrega un torneo con Exito?
            return resultado > 0;
        }

        public async Task<bool> EditarTorneo(CrudTorneoDTO torneo)
        {
            var sqlUpdate =
                @"UPDATE Torneos 
                SET FyHInicioT=@FyHInicioT, FyHFinT=@FyHFinT, Estado=@Estado, IdPaisRealizacion=@IdPaisRealizacion, PartidaActual=@PartidaActual, 
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
