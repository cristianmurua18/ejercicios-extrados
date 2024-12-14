using APP.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APP.Entities.Daos
{
    public interface IDAOLibro
    {

        public string InsertLibro(Libro libro);

        public List<Libro> GetLibroList();

        public string NuevoPrestamo(Prestamo prestamo);

        public List<Usuario> GetUsuarios();

        public List<Prestamo> GetListPrestamos();

        public string ModificarLibro(int id);


    }
}
