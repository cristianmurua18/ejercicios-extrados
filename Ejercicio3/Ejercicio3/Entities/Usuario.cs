using MySqlConnector;
using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;

namespace Ejercicio3.Entities
{
    public class Usuario
    {
        public int Id { get; set; }

        public string? Nombre { get; set; }

        public int Edad { get; set; }

        public string Cadena { get; set; }


        public Usuario()
        {
            Cadena = "Server=localhost;Database=backend;User Id=root;Password=678123ay+;";
        }

        public IEnumerable<Usuario> ListaUsuarios()
        {
            string query = "select * from usuarios";

            using (var conexion = new MySqlConnection(Cadena))
            {
                var personas = conexion.Query<Usuario>(query).ToList();

                return personas;
            }

        }

        public Usuario GetUsuarioPorId(int id)
        {
            string query = "select * from usuarios where id = @id";

            string conection = "Server=localhost;Database=backend;User Id=root;Password=678123ay+;";

            using (var conexion = new MySqlConnection(conection))
            {
                var usuario = conexion.QueryFirstOrDefault<Usuario>(query, new { Id = id });

                return usuario;
            }

        }




    }
}
