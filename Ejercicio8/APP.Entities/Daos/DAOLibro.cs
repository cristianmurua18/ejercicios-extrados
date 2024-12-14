using APP.Entities.Models;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;

namespace APP.Entities.Daos
{
    public class DAOLibro(IDbConnection dbConnection) : IDAOLibro
    {

        private readonly IDbConnection _dbConnection = dbConnection;

        //Seguir aca
        public string InsertLibro(Libro libro)
        {
            var sqlInsert = @"INSERT INTO libros(nombre,disponible) VALUES(@Nombre,@Disponible);";

            var result = _dbConnection.Execute(sqlInsert, libro);

            return $"{result} Libro insertado.";

        }

        public string NuevoPrestamo(Prestamo prestamo)
        {
            //Ver de ubicar el libro y modificar algunas propiedades
            var sqlInsert = @"INSERT INTO prestamos(fechaPrestamo,fechaDevolucion,usuarioId,libroId) VALUES(@FechaPrestamo,@FechaDevolucion,@UsuarioId,@LibroId);";

            _dbConnection.Execute(sqlInsert, prestamo);

            return $"Recuerde la fecha de devolucion: {prestamo.FechaDevolucion}";


        }

        public List<Prestamo> GetListPrestamos()
        {
            var sqlSelect = @"SELECT * FROM prestamos";

            var lst = _dbConnection.Query<Prestamo>(sqlSelect);

            return lst.ToList();

        }

        public List<Libro> GetLibroList()
        {
            var sqlSelect = @"SELECT * FROM libros";

            var lst = _dbConnection.Query<Libro>(sqlSelect);

            return lst.ToList();

        }


        public List<Usuario> GetUsuarios()
        {
            var sqlSelect = @"SELECT * FROM usuario";

            var lst = _dbConnection.Query<Usuario>(sqlSelect);

            return lst.ToList();

        }

        public string ModificarLibro(int id)
        {
            var sqlUpdate = @"UPDATE libros SET Disponible='false' WHERE Id=@Id;";
            //Ver que funciona
            var res = _dbConnection.Execute(sqlUpdate);

            return $"{res} Libro/s modificado/s.";
        }


    }
}
