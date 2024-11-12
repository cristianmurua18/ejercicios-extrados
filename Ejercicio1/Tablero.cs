namespace Ejercicio1
{
    public class Tablero
    {
        //Tamaño del tablero
        public int Tamanio { get; set; }

        //Arreglo bidimensional que representa el tablero, lo declaro en forma de matriz de dos dimensiones(filas,columnas)
        public int[,] Matriz { get; set; }

        //Metodo constructor
        public Tablero(int tamanio)
        {
            Tamanio = tamanio;
            //Instancio el arreglo
            Matriz = new int[tamanio, tamanio];
        }

        //METODOS:
        
        //Llama a ColocarReinas para intentar colocar las reinas y luego imprime el tablero si se encuentra una solución.
        public void BuscarSolucion()
        {
            Console.WriteLine(); //Dejar espacio en blanco
            //Llamo al metodo iniciando en la fila 0
            if (ColocarReinas(0))
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
        public bool ColocarReinas(int fila)
        {
            if (fila >= Tamanio)
            {
                return true; // Todas las reinas han sido colocadas
            }

            for (int col = 0; col < Tamanio; col++)
            {
                if (EsSeguro(fila, col))
                {
                    Matriz[fila, col] = 1; // Colocar la reina
                    if (ColocarReinas(fila + 1)) // Intentar colocar la siguiente reina, recursividad
                    {
                        return true;
                    }
                    Matriz[fila, col] = 0; // Retroceder, backtracking, sacar la reina
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
                if (Matriz[i, col] == 1)
                {
                    return false;
                }
            }

            // Comprobar la diagonal superior izquierda
            for (int i = fila, j = col; i >= 0 && j >= 0; i--, j--)
            {
                if (Matriz[i, j] == 1)
                {
                    return false;
                }
            }

            // Comprobar la diagonal superior derecha
            for (int i = fila, j = col; i >= 0 && j < Tamanio; i--, j++)
            {
                if (Matriz[i, j] == 1)
                {
                    return false;
                }
            }

            return true; // Es seguro colocar la reina
        }

        //Imprime el tablero en la consola, usando 'Q' para las reinas y '.' para las casillas vacías.
        public void ImprimirMatriz()
        {
            for (int i = 0; i < Tamanio; i++)
            {
                for (int j = 0; j < Tamanio; j++)
                {
                    if (Matriz[i, j] == 1)
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
        }
    }
}
