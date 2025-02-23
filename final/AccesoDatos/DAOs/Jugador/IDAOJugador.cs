using Entidades.DTOs.Cruds;
using Entidades.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccesoDatos.DAOs.Jugador
{
    public interface IDAOJugador
    {
        public Task<int> CrearMazo(int userId, string nombreMazo);
        public Task<bool> RegistrarCartas(CrudMazoCartasDTO cartas, int idTorneo, int userId);


    }
}
