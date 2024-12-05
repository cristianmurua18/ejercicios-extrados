Curso de backend con C# y .NET

# EJERCICIOS

UNO: (Ejercicio1)

REQUERIMIENTO: 

->Crear un programa que me indique, de que forma, poner 8 reinas en un tablero de ajedrez, de tal forma que ninguna reina pueda comerse a otra.
Subir la solucion a github, enviar el link con el resultado.

------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

DOS: (Ejercicio2)
“El reto de las 8 piezas”

Siguiendo la linea del ejercicio anterior:

REQUIRIMIENTOS:

->Generar un método “Resolver8Piezas(IPieza pieza)” o “Resolver8Piezas(Pieza pieza)” que pueda colocar 8 de la pieza indicada en un tablero de ajedrez sin que ninguna pieza pueda comerse a ninguna otra.
->Generar múltiples clases para piezas de ajedrez, que implementen la interfaz “IPieza” o que hereden de “Pieza”
->El método “Resolver8Piezas” debe poder, sin ningún cambio a su codigo, agarrar cualquier pieza ya existente o nuevamente creada, ya sea una pieza del ajedrez real o una pieza ficticia, y resolver el problema o indicar que para esta pieza, el problema no tiene solución.
->Este método debe estar en una clase que es parte de una biblioteca de clases
->El proyecto compilado (el que se ejecuta, el que tiene el program.cs) debe llamar a la clase de la biblioteca y utilizarla.

--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

TRES: (Ejercicio3) - Uso de Dapper (como micro-ORM) y la clase MySqlConnection.

REQUIRIMIENTOS: 

-> Crear una base de datos con una sola tabla: “Usuarios”
-> Esta tabla deberá tener Id, Nombre y Edad
-> Crear 2 funciones: una que traiga la lista de usuarios de la tabla Usuarios y la muestre por pantalla, y una que busque y traiga la información de un usuario a partir de su Id
-> Crear un DAO, en una biblioteca de clases aparte, que permita hacer un CRUD (Create - Read - Update - Delete, crear - leer - actualizar - borrar) a la tabla “Usuarios”.

-> Se deberá enviar las variables de tal forma que nos proteja de SQL injection
-> Se deberá tener en cuenta el uso de “using” para no dejar conexiones abiertas con la base de datos
-> El borrado de datos debe ser lógico

->Consumir el DAO de la biblioteca desde el program.cs

----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

CUATRO: (Ejercicio4) - APIREST y Patron de diseño Singleton

Generar una API WEB REST de ASP.NET CORE que pueda guardar, obtener o actualizar usuarios.

REQUERIMIENTOS: 
-> La API debe de poder buscar un usuario por email.
-> Cada usuario tiene un mail,un nombre, edad.
-> Los usuarios se deben guardar en una base de datos.
-> La API debe controlar que la edad > 14,
-> La API debe controlar que el email sea de GMAIL (usar string.Contains())
-> Los DAO deben ser todos singletons y deben estar en su propia biblioteca de clases.

----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

CINCO: (Ejercicio5) - Encriptacion y hasheo

REQUERIMIENTOS: 
-modificar el ejercicio de la clase anterior, para que al guardar un usuario, la contraseña del mismo quede hasheada en la base de datos.
-para esto, buscar una librería para hashear la contraseña
-asegurarse que la librería hashee la contraseña de acuerdo con las recomendaciones de la OWASP
-al llamar al endpoint de obtener usuario, se debe pedir la contraseña y se debe verificar que esta sea correcta.

----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

SEIS: (Ejercicio6) - JWT, Autorizacion y Autenticacion

Crear una API con siguientes endpoints:
-Registro de usuario, que guarde el usuario con la contraseña hasheada,
-Login de usuario, que valida pass y contraseña y devuelve un token JWT junto con la informaicon del usuario (en un objeto)
-Endpoint para obtener la informacion de un usuario cualquiera, obtenido por atributo, este endpoint solo debe poder ser llamado por un usuario logueado.

----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

SIETE: (Ejercicio7) - CORS y patron Options

del ejercicio anterior, agregar CORS y mover las llaves del JWT al archivo appsettings.json y consumirlo desde allí, tanto en el program.cs como en el controlador