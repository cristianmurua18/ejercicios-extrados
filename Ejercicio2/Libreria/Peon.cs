namespace Libreria
{
    public class Peon : IPieza
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
            // Usamos un bucle para colocar los peones en filas alternas
            for (int i = fila; i < 1; i++)
            {
                for (int columna = 0; columna < 8; columna++)
                {
                    // Colocamos un peón en la posición (fila, columna)
                    Tablero[fila, columna] = 1;
                }
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
                        Console.Write(" P ");
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
