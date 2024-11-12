using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Libreria
{
    public interface IPieza
    {


        public void BuscarSolucion();

        public bool ColocarPieza(int fila);

        public bool EsSeguro(int fila, int col);

        public void ImprimirMatriz();


    }
}
