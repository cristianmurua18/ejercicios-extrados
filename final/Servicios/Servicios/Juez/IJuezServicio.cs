using Entidades.DTOs.Jugadores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Servicios.Servicios.Juez
{
    public interface IJuezServicio
    {
        public Task<bool> OficializarResultados();
        public Task<bool> DescalificarJugador(JugadorDescalificadoDTO jugadorDescalificado);

    }
}
