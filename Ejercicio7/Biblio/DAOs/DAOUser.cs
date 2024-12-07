using APP.Entities.Models;
using Dapper;
using Microsoft.AspNetCore.Identity;
using System.Data;

namespace APP.Entities.DAOs
{
    public class DAOUser : IDAOUser
    {

        private readonly IDbConnection _dbConnection;

        public DAOUser(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }


        public List<UserModel> ObtenerListadoUsuarios()
        {
            var sqlSelect = @"SELECT * FROM usuario";

            var lst = _dbConnection.Query<UserModel>(sqlSelect);

            return lst.ToList();
            //Si no hay usuarios me devulve lista vacia
        }


        public UserModel ObtenerUsuarioPorEmail(string email)
        {
            //Ver como recibir el parametro para la consulta. OK
            var sqlSelect = @"SELECT * FROM usuario WHERE emailaddress = @email;";

            var usr = _dbConnection.QueryFirstOrDefault<UserModel>(sqlSelect, new { EmailAddress = email });

            return usr;

        }


        public void AgregarUsuario(UserModel usuario)
        {
            try
            {   //Validacion para que no me permita ingresar contraseña vacia
                if (string.IsNullOrEmpty(usuario.PasswordHash))
                {
                    throw new ArgumentException("La propiedad 'PasswordHash' no puede ser nula o vacía.");
                }

                //Hago el HASEO de la contraseña. OK
                HashPassword(usuario);

                //Guardo en BD si cumple ambas condiciones. Validacion aqui o en el servicio?
                if (usuario.Age >= 18 & usuario.EmailAddress.Contains("@gmail.com"))
                {
                    var sqlInsert = @"INSERT INTO usuario(username,passwordhash,emailaddress,rol,lastname,firstname,age) VALUES(@UserName,@PasswordHash,@EmailAddress,@Rol,@LastName,@FirstName,@Age);";

                    var result = _dbConnection.Execute(sqlInsert, usuario);

                }
                else
                {
                    throw new Exception("La edad debe ser mayor o igual a 18 y el correo debe ser de Gmail");
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }


        public string ModificarUsuarioPorID(UserModel usuario)
        {

            //HashPassword(usuario);

            var sqlUpdate = @"UPDATE usuario SET Nombre=@nombre, Email=@email, PasswordHash=@passwordhash, Age=@edad WHERE Id=@Id;";
            //OJO que deberia hashear la contraseña
            var res = _dbConnection.Execute(sqlUpdate, usuario);

            return $"{res} Usuario/s modificado/s.";

        }



        public string BorrarUsuarioPorID(UserModel usuario)
        {

            var sqlDelete = @"DELETE FROM usuario WHERE Id=@Id;";

            var res = _dbConnection.Execute(sqlDelete, new { usuario.Id });

            return $"Registros eliminados: {res}";

        }


        public string HashPassword<T>(T usuario) where T : class
        {
            try
            {
                //Verifico que la clase tenga la propiedad PasswordHash
                var property = typeof(T).GetProperty("PasswordHash");

                if (property == null || !property.CanWrite || !property.CanRead)
                {
                    throw new InvalidOperationException("El tipo genérico debe tener una propiedad 'PasswordHash'.");
                }

                PasswordHasher<T> pass = new();

                string currentPassword = property.GetValue(usuario)?.ToString();

                //Hash de la contraseña, y sobreescribo valor

                var hashedpassword = pass.HashPassword(usuario, currentPassword);
                property.SetValue(usuario, hashedpassword);

                return hashedpassword;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }

        }

    }
}
