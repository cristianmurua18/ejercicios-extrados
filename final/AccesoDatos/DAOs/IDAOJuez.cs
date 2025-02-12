using Entidades.DTOs.Jugadores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccesoDatos.DAOs
{
    public interface IDAOJuez
    {
        public Task<bool> OficializarResultados();
        public Task<bool> DescalificarJugador(JugadorDescalificadoDTO jugadorDescalificado);

    }

}
