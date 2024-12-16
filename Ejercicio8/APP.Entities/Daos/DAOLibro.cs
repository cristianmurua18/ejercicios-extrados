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
            //Solo se debe cargar el usuarioID y libroID
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

        public string UpdateLibro(int id)
        {
            var sqlUpdate = @"UPDATE libros SET disponible='false' WHERE id=@id;";
            //Ver si funciona, NO modifica
            var res = _dbConnection.Execute(sqlUpdate);

            return $"{res} Libro/s modificado/s.";
        }


    }
}
