

using ClassLibrary1.Entities;
using ClassLibrary1.Servicios;
using ClassLibrary1.Utils;
using Ejercicio3.Entities;

Console.WriteLine("Hola, Bienvenido!");
/*     CODIGO PARA PARTE 1 DEL EJERCICIO
var usuarios = new Usuario();

var lista = usuarios.ListaUsuarios();

Console.WriteLine("Lista de usuarios:");

foreach (var item in lista)
{
    Console.WriteLine($"ID: {item.Id}, Nombre: {item.Nombre}, Edad: {item.Edad}");
}


Console.WriteLine("------------------------------------------------------------");

Console.WriteLine("Obtener usuario por Id");

var usuario = new Usuario();

var user = usuario.GetUsuarioPorId(3);

Console.WriteLine($"ID: {user.Id}, Nombre: {user.Nombre}, Edad: {user.Edad}");

*/

// CODIGO PARA PARTE DOS DEL EJERCICIO


while (true)
{
    //Para borrar la pantalla en cada vuelva del bucle
    Console.Clear();

    //Creo un metodo que se llama cada vez que necesito una opcion
    //Como el metodo devuelve un numero, lo guardo en la varible 
    var opcion = Options();

    //Convierto la opcion en un OptionEnum a traves de un casteo(lo trata como si fuera ese tipo de dato)
    var optionEnum = (OptionEnum)opcion;
    //Evuluo si me ingresa un 5 para salir del bucle
    if (opcion == 6)
        return;

    
    //Varible que guarda un mensaje
    string message = optionEnum switch
    {
        //Cada metodo devuelve un string
        OptionEnum.Add => DAOUsuarios.CrearUsuarios(),
        OptionEnum.Update => DAOUsuarios.ModificarUsuarios(),
        OptionEnum.Delete => DAOUsuarios.BorrarUsuarios(),
        OptionEnum.GetAll => DAOUsuarios.ObtenerListadoUsuarios(),
        OptionEnum.GetById => DAOUsuarios.ObtenerUsuarioPorId(),
        OptionEnum.Exit => "Salir",
        _ => "Opcion no valida"

    };
    //Para imprimir por consola la informacion(el mensaje)
    Console.WriteLine(message);
    Console.ReadLine();

}


//Metodo static que devuelve un numero(por el int)
static int Options()
{
    //Para tratar de controlar la exepcion
    try
    {
        Console.WriteLine("Bienvenido al CRUD de Usuarios.");
        Console.WriteLine("1. Crear un Usuario");
        Console.WriteLine("2. Editar un Usuario");
        Console.WriteLine("3. Eliminar un Usuario");
        Console.WriteLine("4. Obtener Listado de Usuarios");
        Console.WriteLine("5. Obtener un Usuario por Id");
        Console.WriteLine("6. Salir");

        Console.Write("Ingrese opcion: ");
        var opcion = Convert.ToInt16(Console.ReadLine());
        Console.WriteLine();
        return opcion;
    }
    catch (Exception)
    {
        Console.WriteLine("Formato incorrecto de numero. Intente de nuevo");
        return 0;
    }

}