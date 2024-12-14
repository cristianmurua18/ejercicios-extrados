using APP.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APP.Services.Services
{
    public interface ILibroService
    {

        public List<Libro> GetAllLibros();

        public string InsertLibro(Libro libro);

        public string AgregarPrestamo(Prestamo prestamo);

        public List<Usuario> ObtenerUsuarios();

        public string GenerateJwt(Usuario user);

        public List<Prestamo> Prestamos();

        public string ModificarLibro(int id);




    }
}
