using Dapper;
using Entidades.DTOs;
using Entidades.DTOs.Cruds;
using Microsoft.IdentityModel.Tokens;
using System.Data;

namespace AccesoDatos.DAOs.Administrador
{
    public class DAOAdministrador(IDbConnection dbConnection) : IDAOAdministrador
    {
        //Propiedad para recibir la conexion a la base de datos
        private readonly IDbConnection _dbConnection = dbConnection;

        public async Task<List<UsuarioDTO>> ObtenerUsuariosPorRol(string rol)
        {
            string sqlJoin =
                @"SELECT *
                FROM Usuarios us
                INNER JOIN Paises pa
                ON us.IdPaisOrigen = pa.PaisID
                WHERE Rol LIKE CONCAT('%', @Rol, '%');";

            var usuarios = await _dbConnection.QueryAsync<UsuarioDTO>(sqlJoin, new { Rol = rol });

            return usuarios.ToList();
        }



        public async Task<List<UsuarioDTO>> ObtenerUsuariosPorNombre(string nombre)
        {
            string sqlJoin =
                @"SELECT *
                FROM Usuarios us
                INNER JOIN Paises pa
                ON us.IdPaisOrigen = pa.PaisID
                WHERE NombreApellido LIKE CONCAT('%', @NombreApellido, '%');";
                                       //Mejora de Codigo ->
            var usuarios = await _dbConnection.QueryAsync<UsuarioDTO>(sqlJoin, new { NombreApellido = nombre});

            return usuarios.ToList();
        }

        public async Task<UsuarioDTO> ObtenerUsuarioPorId(int id)
        {
            string sqlJoin =
                @"SELECT *
                FROM Usuarios us
                INNER JOIN Paises pa
                ON us.IdPaisOrigen = pa.PaisID
                WHERE us.UsuarioID=@UsuarioID;";

            var usuario = await _dbConnection.QuerySingleOrDefaultAsync<UsuarioDTO>(sqlJoin, new { UsuarioID = id });

            return usuario!;
        }

        public async Task<bool> RegistrarUsuario(CrudUsuarioDTO usuario)
        {
            var sqlInsert = @"INSERT
                INTO Usuarios(NombreApellido,Alias,IdPaisOrigen,Email,NombreUsuario,Contraseña,FotoAvatar,Rol,CreadoPor, Activo) 
                VALUES(@NombreApellido,@Alias,@IdPaisOrigen,@Email,@NombreUsuario,@Contraseña,@FotoAvatar,@Rol,@CreadoPor,@Activo);";

            var result = await _dbConnection.ExecuteAsync(sqlInsert, usuario);

            return result > 0;

        }

        public async Task<bool> ActualizarUsuarioPorID(CrudUsuarioDTO usuario)
        {       //SIN tocar contraseña
            var sqlUpdate =
                @"UPDATE Usuarios 
                SET NombreApellido=@NombreApellido, Alias=@Alias, IdPaisOrigen=@IdPaisOrigen, Email=@Email, NombreUsuario=@NombreUsuario, 
                FotoAvatar=@FotoAvatar, Rol=@Rol, CreadoPor=@CreadoPor, Activo=@Activo
                WHERE UsuarioID=@UsuarioID;";

            var resultado = await _dbConnection.ExecuteAsync(sqlUpdate, usuario);
            //QuerySingleOrDefaultAsync?
            return resultado > 0;
        }

        public async Task<bool> BorrarUsuarioPorID(int id)
        {       //Implementacion de borrado logico, capaz mejor sola reciba un id
            var sqlDelete =
                @"DELETE Usuarios WHERE UsuarioID=@UsuarioID AND Activo=0;";

            var resultado = await _dbConnection.ExecuteAsync(sqlDelete, new { UsuarioID = id });

            return resultado > 0;
        }

        public async Task<List<TorneoDTO>> VerTorneosYPartidas()
        {
            string sqlJoin =
                @"SELECT *
                FROM Torneos tor
                INNER JOIN Partidas par
                ON tor.PartidaActual = par.PartidaID;";
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
