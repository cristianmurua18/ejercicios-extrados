using AccesoDatos.DAOs.Juez;
using Entidades.DTOs.Jugadores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Servicios.Servicios.Juez
{
    public class JuezServicio(IDAOJuez daoJuez) : IJuezServicio
    {
        private readonly IDAOJuez _daoJuez = daoJuez;

        public async Task<bool> OficializarResultados()
        {
            return await _daoJuez.OficializarResultados();
        }
        public async Task<bool> DescalificarJugador(JugadorDescalificadoDTO jugadorDescalificado)
        {
            return await _daoJuez.DescalificarJugador(jugadorDescalificado);
        }
    }
}
