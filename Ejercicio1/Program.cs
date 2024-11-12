
//Arreglo de arreglos. Puede poner muchos arreglos y de diferentes tamaños


//int[][] arrayJagged = new int[2][] {
//new int[4] { 0,1,2,3},
//new int[3] { 4,5,6}
//};

////Recorrer
////Cuento la cantidad de arreglos a recorrer
//for (int i = 0; i < arrayJagged.Length; i++)
//{
//    //Recorro cada arreglo
//    for (int j = 0; j < arrayJagged[i].Length; j++)
//        Console.Write($"{arrayJagged[i][j]} ");
//    Console.WriteLine();
//}

//-------------------------------------------------------------------------------------------------------------------

using Ejercicio1;


int n = 8; // Número de reinas
Tablero tabla = new Tablero(n);
tabla.BuscarSolucion();