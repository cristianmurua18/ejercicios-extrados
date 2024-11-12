using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Libreria
{
    public class Reina : IPieza
    {

        public int[,]? Tablero { get; set; }

        //METODOS:

        //Llama a ColocarPieza para intentar colocar las reinas y luego imprime el tablero si se encuentra una solución.
        public void BuscarSolucion()
        {
            Tablero = new int[8, 8];
            
            //Llamo al metodo iniciando en la fila 0
            if (ColocarPieza(0))
            {
                ImprimirMatriz();
            }
            else
            {
                Console.WriteLine("No hay solución posible.");
            }
        }


        //Intenta colocar una reina en cada fila.Si se coloca de forma segura, se llama recursivamente para colocar la siguiente reina.
        //Si no se puede colocar, retrocede (backtracking).
        public bool ColocarPieza(int fila)
        {
            if (fila >= 8)
            {
                return true; // Todas las reinas han sido colocadas
            }

            for (int col = 0; col < 8; col++)
            {
                if (EsSeguro(fila, col))
                {
                    Tablero[fila, col] = 1; // Colocar la reina
                    if (ColocarPieza(fila + 1)) // Intentar colocar la siguiente reina, recursividad
                    {
                        return true;
                    }
                    Tablero[fila, col] = 0; // Retroceder, backtracking, sacar la reina
                }
            }
            return false; // No se pudo colocar la reina
        }

        //Verifica si es seguro colocar una reina en la posición(fila, columna) comprobando la columna y las dos diagonales.
        public bool EsSeguro(int fila, int col)
        {
            // Comprobar la columna
            for (int i = 0; i < fila; i++)
            {
                if (Tablero[i, col] == 1)
                {
                    return false;
                }
            }

            // Comprobar la diagonal superior izquierda
            for (int i = fila, j = col; i >= 0 && j >= 0; i--, j--)
            {
                if (Tablero[i, j] == 1)
                {
                    return false;
                }
            }

            // Comprobar la diagonal superior derecha
            for (int i = fila, j = col; i >= 0 && j < 8; i--, j++)
            {
                if (Tablero[i, j] == 1)
                {
                    return false;
                }
            }

            return true; // Es seguro colocar la reina
        }

        //Imprime el tablero en la consola, usando 'Q' para las reinas y '.' para las casillas vacías.
        public void ImprimirMatriz()
        {
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (Tablero[i, j] == 1)
                    {
                        Console.Write(" Q ");
                    }
                    else
                    {
                        Console.Write(" . ");
                    }
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }
    }



}

