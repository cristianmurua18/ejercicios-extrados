using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Libreria
{
    public class Auto : IPieza
    {
        public int[,]? Tablero { get; set; }

        public void BuscarSolucion()
        {
            Tablero = new int[8, 8];
            ColocarPieza(0);
            ImprimirMatriz();
        }

        public bool ColocarPieza(int fila)
        {
            //Diagonal principal
            for (int i = fila; i < 8; i++)
            {
                Tablero[i, i] = 1;
            }

            //Diagonal secundaria
            for (int i = 0; i < 8; i++)
            {
                   
                Tablero[i, 7-i] = 1;
                    
            }
            return true;
        }

        public bool EsSeguro(int fila, int col)
        {
            return true;
        }

        public void ImprimirMatriz()
        {
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (Tablero[i, j] == 1)
                    {
                        Console.Write(" A ");

                    }
                    else
                    {
                        Console.Write(" . ");
                    }
                }
                Console.WriteLine();
            }
        }
    }
}
