using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades.Modelos
{
    public class Partida
    {
        //Solo partida
        public int PartidaID { get; set; }
        public DateTime FyHInicioP { get; set; }
        public DateTime FyHFinP { get; set; }
        public string? Ronda { get; set; }
        public int JugadorDerrotado { get; set; }
        public int JugadorVencedor {  get; set; }



    }
}
