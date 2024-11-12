namespace Libreria
{
    public class Diagonal : IPieza
    {

        public int[,]? Tablero {  get; set; }

        public void BuscarSolucion()
        {
            Tablero = new int[8, 8];
            ColocarPieza(0);
            ImprimirMatriz();
        }

        public bool ColocarPieza(int fila)
        {
            
            for (int i = fila; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (i != j)
                    {
                        Tablero[i, j] = 0;
                    }
                    else
                    {
                        Tablero[i, j] = 1;
                    }
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
                    if (Tablero[i, j] == 0)
                    {
                        Console.Write(" . ");
                       
                        if (j ==7) { Console.WriteLine(); }   
                    }
                    else
                    {
                        Console.Write(" T ");
                    }
                }
            }
            Console.WriteLine();
        }
    }
}
