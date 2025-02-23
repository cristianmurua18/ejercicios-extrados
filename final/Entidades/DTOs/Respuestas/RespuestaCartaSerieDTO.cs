using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades.DTOs.Respuestas
{
    public class RespuestaCartaSerieDTO
    {
        public int CartaID { get; set; }
        public string? NombreCarta { get; set; }
        public int SerieID { get; set; }
        public string? NombreSerie { get; set; }
    }
}
