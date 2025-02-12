using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades.DTOs.Cruds
{
    public class CrudMazoDTO
    {
        public int MazoID { get; set; }
        public string? Nombre { get; set; }
        public int JugadorCreador { get; set; }

    }
}
