using Biblio.Models;
using Dapper;
using MySqlConnector;
using System.Security.Policy;

namespace Biblio.DAOs
{
    public class DAOUsuario
    {

        private static DAOUsuario _instance = null;

        private DAOUsuario() { }

        public static DAOUsuario Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new DAOUsuario();
                return _instance;
            }
        }


        private static string? Cadena { get; set; } = "Server=localhost;Database=backend;User Id=root;Password=678123ay+;";


        public IEnumerable<Usuario> ObtenerListadoUsuarios()
        {
            using (var conexion = new MySqlConnection(Cadena))
            {
                string message = string.Empty;

                var sqlSelect = @"SELECT * FROM usuario";

                var lst = conexion.Query<Usuario>(sqlSelect);

                if (lst.Count() == 0)
                    Console.WriteLine("No hay usuarios disponibles");

                return lst;

            }
        }


        public Usuario ObtenerUsuarioPorEmail(string email)
        {
            using (var conexion = new MySqlConnection(Cadena))
            {
                //Ver como recibir el parametro para la consulta
                var sqlSelect = @"SELECT * FROM usuario WHERE email = @Email;";

                var usr = conexion.QueryFirstOrDefault<Usuario>(sqlSelect, new { Email = email });

                return usr;
            }
        }

//al guardar un nuevo usuario, se debe primero hashear la contraseña y lo que se guarda en la base de datos es el hash de la contraseña,
        //no la contraseña en si

        public string AgregarUsuario(Usuario usuario)
        {

            using (var conexion = new MySqlConnection(Cadena))
            {
                if (usuario.Edad > 14 & usuario.Email.Contains("@gmail.com"))
                {
                    var sqlInsert = @"INSERT INTO usuario(nombre,email,edad) VALUES(@nombre,@email,@edad);";

                    var result = conexion.Execute(sqlInsert, usuario);

                    return $"{result} Usuario/s creado/s.";
                    //Podria devolver el usuario directamente
                }
                else
                {
                    throw new Exception("La edad debe ser mayor a 14 y el correo debe ser de Gmail");
                }

            }

        }


        public string ModificarUsuarioPorID(Usuario usuario)
        {
            //Ubica por ID

            using (var conexion = new MySqlConnection(Cadena))
            {
                var sqlUpdate = @"UPDATE usuario SET Email=@email, Edad=@edad, Nombre=@nombre WHERE Id=@Id;";

                var res = conexion.Execute(sqlUpdate, usuario);

                return $"{res} Usuario/s modificado/s.";

            }


        }



        public string BorrarUsuarioPorID(Usuario usuario)
        {   //Ver como ubicar el usario para luego borrar 

            using (var conexion = new MySqlConnection(Cadena))
            {

                var sqlDelete = @"DELETE FROM usuario WHERE Id=@Id;";

                var res = conexion.Execute(sqlDelete, new { usuario.Id });

                return $"Registros eliminados: {res}";

            }

        }
    }
}
