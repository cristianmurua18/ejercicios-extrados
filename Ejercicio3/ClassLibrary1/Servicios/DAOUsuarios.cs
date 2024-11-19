//using Ejercicio3.Entities.Usuarios;
using ClassLibrary1.Entities;
using Dapper;
using MySqlConnector;

namespace ClassLibrary1.Servicios
{
    public static class DAOUsuarios
    {
        private static string? Cadena { get; set; } = "Server=localhost;Database=backend;User Id=root;Password=678123ay+;";


        public static string CrearUsuarios()
        {
            try
            {
                using (var conexion = new MySqlConnection(Cadena))
                {
                    Console.WriteLine("Inserte el Id del nuevo Usuario: ");
                    var id = Convert.ToInt16(Console.ReadLine());

                    Console.WriteLine("Inserte el Nombre del nuevo Usuario: ");
                    string nombre = Console.ReadLine();

                    Console.WriteLine("Inserte la Edad del nuevo Usuario: ");
                    int edad = Convert.ToInt16(Console.ReadLine());

                    Console.WriteLine("Inserte el Estado del nuevo Usuario: true o false");
                    string estado = Console.ReadLine();

                    var sqlInsert = "insert into usuarios values(@id,@nombre,@edad,@estado);";

                    var result = conexion.Execute(sqlInsert, new { Id = id, Nombre = nombre, Edad = edad, Estado = estado });

                    return $"Usuario creado con exito.";

                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("ERROR. No fue posible insertar.");
                return $"{ex.Message}";
            }
        }


        public static string ObtenerListadoUsuarios()
        {
            using (var conexion = new MySqlConnection(Cadena))
            {
                string message = string.Empty;

                var sqlSelect = "select * from usuarios";

                var lst = conexion.Query<Usuarios>(sqlSelect);

                Console.WriteLine("La lista de usuarios disponibles es: ");

                if (lst.Count() == 0)
                    message = "No hay usuarios disponibles";

                foreach (var item in lst)
                {
                    message += $"\nId: {item.Id} \nNombre: {item.Nombre} \nEdad: {item.Edad} \nEstado: {item.Estado} \n----------------------------------------------------------";
                }
                return message;

            }
        }

        public static string ObtenerUsuarioPorId()
        {

            try
            {
                using (var conexion = new MySqlConnection(Cadena))
                {
                    Console.WriteLine("Inserte el Id del Usuario a buscar: ");
                    var id = Convert.ToInt16(Console.ReadLine());

                    var sqlSelect = "select * from usuarios where id = @id;";

                    var usr = conexion.QueryFirstOrDefault<Usuarios>(sqlSelect, new { Id = id });

                    if (usr == null)
                    {
                        return $"Usuario no encontrado. Pruebe con otro Id.";
                    }
                    else
                    {
                        return $"Id: {usr.Id}, Nombre: {usr.Nombre}, Edad: {usr.Edad}, Estado: {usr.Estado}";
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("ERROR. No fue posible buscar.");
                return ex.Message;
            }
        }


        public static string ModificarUsuarios()
        {

            try
            {
                //ver de ubicarlo por id y luego cambiar de valor
                using (var conexion = new MySqlConnection(Cadena))
                {
                    Console.WriteLine("Inserte el Id del Usuario a buscar: ");
                    var id = Convert.ToInt16(Console.ReadLine());

                    var sqlSelect = "select * from usuarios where id = @id;";

                    var usr = conexion.QueryFirstOrDefault<Usuarios>(sqlSelect, new { Id = id });

                    if (usr == null)
                    {
                        return $"Usuario no encontrado. Pruebe con otro Id.";
                    }
                    else
                    {
                        Console.WriteLine("Inserte el nuevo Nombre del Usuario: ");
                        string nombre = Console.ReadLine();

                        Console.WriteLine("Inserte la nueva Edad del Usuario: ");
                        int edad = Convert.ToInt16(Console.ReadLine());

                        Console.WriteLine("Inserte el nuevo Estado del Usuario: ");
                        string estado = Console.ReadLine();

                        var sqlUpdate = "UPDATE usuarios SET Nombre=@nombre, Edad=@edad, Estado=@estado where id=@id;";

                        var res = conexion.Execute(sqlUpdate, new { Id = id, Nombre = nombre, Edad = edad, Estado = estado });

                        Console.WriteLine("Usuario modificado con exito. Sus datos anteriores eran: ");

                        return $"Id: {usr.Id}, Nombre: {usr.Nombre}, Edad: {usr.Edad}, Estado: {usr.Estado}";
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("ERROR. No fue posible modificar.");
                return ex.Message;
            }

        }

        //Aclaracion a la funcionalidad del metodo. Solo esta permitido borrar si estado es false(Usuarios dados de baja, a traves de su Id).
        //Regla del campo estado en la BD. true = activo, false = de baja.
        public static string BorrarUsuarios()
        {
            try
            {
                using (var conexion = new MySqlConnection(Cadena))
                {
                    Console.WriteLine("Inserte el Id del Usuario a borrar: ");
                    var id = Convert.ToInt16(Console.ReadLine());
                    
                    var sqlUpdate = "DELETE FROM usuarios WHERE id=@id AND estado='false';";

                    var res = conexion.Execute(sqlUpdate, new { Id = id });

                    return $"Registros eliminados: {res}";

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al intentar borrar usuario.");
                return ex.ToString();
            }

        }

    }
}
