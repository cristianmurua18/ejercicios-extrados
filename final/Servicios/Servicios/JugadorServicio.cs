using AccesoDatos.DAOs;
using Entidades.DTOs.Cruds;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Servicios.Servicios
{
    public class JugadorServicio(IDAOJugador daoJugador) : IJugadorServicio
    {

        private readonly IDAOJugador _daoJugador = daoJugador;

        public async Task<int> CrearMazo(CrudMazoDTO mazo)
        {
            return await _daoJugador.CrearMazo(mazo);

        }
    
        public async Task<bool> RegistrarCartas(CrudMazoCartasDTO cartas)
        {
            return await _daoJugador.RegistrarCartas(cartas);

        }

    }
}
