using Biblio.Models;
using Dapper;
using Microsoft.AspNetCore.Identity;
using MySqlConnector;

namespace Biblio.DAOs
{
    /*
     1) Se agrega funcion para Hash de contraseña antes de enviar informacion a BD
     2) Se agrega funcion para comparar Hash con contraseña ingresada por usuario para verificar credenciales
     */

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


        private static string? Cadena { get; set; } = "Server=localhost;Database=backend;User Id=root;Password=678123ay+;Allow User Variables=true;";


        public IEnumerable<Usuario> ObtenerListadoUsuarios()
        {
            //al loguear el usuario, o al requerir ver si la contraseña le pertenece al usuario, se debe hashear la contraseña que se ingreso,
            //y comprar con el hash en la base de datos

            using (var conexion = new MySqlConnection(Cadena))
            {
                var sqlSelect = @"SELECT Id,Nombre,Email,Edad FROM usuario";

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
                //Ver como recibir el parametro para la consulta. OK
                var sqlSelect = @"SELECT Id,Nombre,Email,Edad FROM usuario WHERE email = @Email;";

                var usr = conexion.QueryFirstOrDefault<Usuario>(sqlSelect, new { Email = email });

                return usr;

            }
        }

        //al guardar un nuevo usuario, se debe primero hashear la contraseña y lo que se guarda en la base de datos es el hash de la contraseña,
        //no la contraseña en si
        public string AgregarUsuario(Usuario usuario)
        {
            try
            {
                using (var conexion = new MySqlConnection(Cadena))
                {
                  
                    HashPassword(usuario);

                    //Guardo en BD
                    if (usuario.Edad > 14 & usuario.Email.Contains("@gmail.com"))
                    {
                        var sqlInsert = @"INSERT INTO usuario(nombre,email,passwordhash,edad) VALUES(@Nombre,@Email,@PasswordHash,@Edad);";

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
            catch (Exception ex)
            {
                return $"{ex.Message}";
            }

        }


        public string ModificarUsuarioPorID(Usuario usuario)
        {
            //Ubica por ID

            using (var conexion = new MySqlConnection(Cadena))
            {
                HashPassword(usuario);

                var sqlUpdate = @"UPDATE usuario SET Nombre=@nombre, Email=@email, PasswordHash=@passwordhash, Edad=@edad WHERE Id=@Id;";
                //OJO que deberia hashear la contraseña. OK
                var res = conexion.Execute(sqlUpdate, usuario);

                return $"{res} Usuario/s modificado/s.";

            }


        }



        public string BorrarUsuarioPorID(Usuario usuario)
        {
            using (var conexion = new MySqlConnection(Cadena))
            {
                var sqlDelete = @"DELETE FROM usuario WHERE Id=@Id;";

                var res = conexion.Execute(sqlDelete, new { usuario.Id });

                return $"Registros eliminados: {res}";

            }

        }


        public string Login(Usuario usuario, string pass)
        {
            using (var conexion = new MySqlConnection(Cadena))
            {

                //Hago la consulta para poder recuperar lo de la BD
                var sqlSelect = @"SELECT Email,PasswordHash FROM usuario WHERE email = @Email;";

                var usr = conexion.QueryFirstOrDefault<Usuario>(sqlSelect, new { usuario.Email,usuario.PasswordHash });
                //Comparo ambas contraseñas

                var verif = VerifyPassword(usr, pass);

                return verif == true ? $"Inicio de sesion exitoso" : $"Inicio de sesion NO exitoso";

            }

        }


        public string HashPassword(Usuario usuario)
        {
            PasswordHasher<Usuario> pass = new PasswordHasher<Usuario>();
            //Hash de la contraseña, y sobreescribo valor
            usuario.PasswordHash = pass.HashPassword(usuario, usuario.PasswordHash);

            return $"Contrasena Hasheada.";
        }

        public bool VerifyPassword(Usuario usuario, string password)
        {
            var pass = new PasswordHasher<Usuario>();
            var result = pass.VerifyHashedPassword(usuario, usuario.PasswordHash, password);
            return result == PasswordVerificationResult.Success;
        }


    }
}
