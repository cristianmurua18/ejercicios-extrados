using Dapper;
using Entidades.DTOs;
using Entidades.DTOs.Cruds;
using Microsoft.IdentityModel.Tokens;
using System.Data;

namespace AccesoDatos.DAOs
{
    public class DAOAdministrador(IDbConnection dbConnection) : IDAOAdministrador
    {
        //Propiedad para recibir la conexion a la base de datos
        private readonly IDbConnection _dbConnection = dbConnection;

        public async Task<List<UsuarioDTO>> ObtenerUsuariosPorNombre(string patron)
        {
            string sqlJoin =
                @"SELECT *
                FROM Usuarios us
                INNER JOIN Paises pa
                ON us.IdPais = pa.PaisID
                WHERE NombreApellido LIKE @patron;";
            //PROBAR
            var usuarios = await _dbConnection.QueryAsync<UsuarioDTO>(sqlJoin, new { NombreApellido = "%" + patron + "%" });

            return usuarios.ToList();
        }

        public async Task<UsuarioDTO> ObtenerUsuarioPorId(int id)
        {
            string sqlJoin =
                @"SELECT *
                FROM Usuarios us
                INNER JOIN Paises pa
                ON us.IdPais = pa.PaisID
                WHERE us.UsuarioID=@id;";
            //PROBAR
            var usuario = await _dbConnection.QuerySingleOrDefaultAsync<UsuarioDTO>(sqlJoin, new { });

            return usuario!;
        }

        public async Task<bool> RegistrarUsuario(CrudUsuarioDTO usuario)
        {
            var sqlInsert = @"INSERT
                INTO Usuarios(NombreApellido,Alias,IdPais,Email,NombreUsuario,Contraseña,FotoAvatar,Rol,CreadoPor, Activo) 
                VALUES(@NombreApellido,@Alias,@IdPais,@Email,@NombreUsuario,@Contraseña,@FotoAvatar,@Rol,@CreadoPor,@Activo);";

            var result = await _dbConnection.ExecuteAsync(sqlInsert, usuario);

            return result > 0;

        }

        public async Task<bool> ActualizarUsuarioPorID(CrudUsuarioDTO usuario)
        {       //SIN tocar contraseña
            var sqlUpdate =
                @"UPDATE Usuarios 
                SET NombreApellido=@NombreApellido, Alias=@Alias, IdPais=@IdPais, Email=@Email, NombreUsuario=@NombreUsuario, 
                FotoAvatar=@FotoAvatar, Rol=@Rol, CreadoPor=@CreadoPor, Activo=@Activo
                WHERE UsuarioID=@UsuarioID;";

            var resultado = await _dbConnection.ExecuteAsync(sqlUpdate, usuario);
            //QuerySingleOrDefaultAsync
            return resultado > 0;
        }

        public async Task<bool> BorrarUsuarioPorID(CrudUsuarioDTO usuario)
        {       //Implementacion de borrado logico
            var sqlDelete =
                @"DELETE Usuarios WHERE UsuarioID=@UsuarioID AND Activo=0;";

            var resultado = await _dbConnection.ExecuteAsync(sqlDelete, usuario);

            return resultado > 0;
        }

        public async Task<List<TorneoDTO>> VerTorneosYPartidas()
        {
            string sqlJoin =
                @"SELECT *
                FROM Torneos to
                INNER JOIN Partidas par
                ON to.PartidaActual = pa.PartidaID;";
            //PARA VER TORNEOS Y PARTIDAS
            var torneos = await _dbConnection.QueryAsync<TorneoDTO>(sqlJoin, new { });

            return torneos.ToList();
        }


        public async Task<bool> CancelarTorneos(int torneoid, string estado)
        {
            var sqlDelete =
                @"UPDATE Torneos 
                SET Estado=@estado
                WHERE TorneoID=@torneoid;";

            var resultado = await _dbConnection.ExecuteAsync(sqlDelete, new { torneoid });

            return resultado > 0;
        }






    }
}
