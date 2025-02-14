using Entidades.DTOs.Cruds;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Servicios.Servicios.Jugador
{
    public interface IJugadorServicio
    {
        public Task<int> CrearMazo(CrudMazoDTO mazo);
        public Task<bool> RegistrarCartas(CrudMazoCartasDTO cartas);
        
    }
}
