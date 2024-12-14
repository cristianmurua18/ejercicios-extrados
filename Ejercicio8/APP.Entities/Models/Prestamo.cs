using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APP.Entities.Models
{
    public class Prestamo
    {
        public int Id { get; set; }

        public DateTime FechaPrestamo {  get; set; }

        public DateTime FechaDevolucion { get; set;}

        public int UsuarioID { get; set; }

        public int LibroID { get; set; }


    }
}
