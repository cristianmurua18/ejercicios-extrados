using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades.DTOs.Jugadores
{
    public class JugadorDescalificadoDTO
    {
        public int DescalificacionID { get; set; }
        public string? Motivo { get; set; }
        public int JuezDescalificador { get; set; }
        public int JugadorID { get; set; }

    }
}
