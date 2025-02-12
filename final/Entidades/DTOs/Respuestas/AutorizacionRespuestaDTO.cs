using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades.DTOs.Respuestas
{
    public class AutorizacionRespuestaDTO
    {
        public string? Token { get; set; }
        public bool Resultado { get; set; }
        public string? Msj { get; set; }

    }
}
