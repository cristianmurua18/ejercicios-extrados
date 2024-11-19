Curso de backend con C# y .NET

# EJERCICIOS

UNO: (Ejercicio1)

Crear un programa que me indique, de que forma, poner 8 reinas en un tablero de ajedrez, de tal forma que ninguna reina pueda comerse a otra.
Subir la solucion a github, enviar el link con el resultado.

----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

DOS: (Ejercicio2)
“El reto de las 8 piezas”

Siguiendo la linea del ejercicio anterior:

->Generar un método “Resolver8Piezas(IPieza pieza)” o “Resolver8Piezas(Pieza pieza)” que pueda colocar 8 de la pieza indicada en un tablero de ajedrez sin que ninguna pieza pueda comerse a ninguna otra.

->Generar múltiples clases para piezas de ajedrez, que implementen la interfaz “IPieza” o que hereden de “Pieza”

->El método “Resolver8Piezas” debe poder, sin ningún cambio a su codigo, agarrar cualquier pieza ya existente o nuevamente creada, ya sea una pieza del ajedrez real o una pieza ficticia, y resolver el problema o indicar que para esta pieza, el problema no tiene solución.

->Este método debe estar en una clase que es parte de una biblioteca de clases

->El proyecto compilado (el que se ejecuta, el que tiene el program.cs) debe llamar a la clase de la biblioteca y utilizarla.

--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

TRES: (Ejercicio3) - Uso de Dapper (como micro-ORM)y la clase MySqlConnection.

-> crear una base de datos con una sola tabla: “Usuarios”
-> esta tabla deberá tener Id, Nombre y Edad
-> crear 2 funciones: una que traiga la lista de usuarios de la tabla Usuarios y la muestre por pantalla, y una que busque y traiga la información de un usuario a partir de su Id
->crear un DAO, en una biblioteca de clases aparte, que permita hacer un CRUD (Create - Read - Update - Delete, crear - leer - actualizar - borrar) a la tabla “Usuarios”.

-> se deberá enviar las variables de tal forma que nos proteja de SQL injection
-> se deberá tener en cuenta el uso de “using” para no dejar conexiones abiertas con la base de datos
-> el borrado de datos debe ser lógico

->Consumir el DAO de la biblioteca desde el program.cs


------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

