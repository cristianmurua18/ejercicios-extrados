using APP.Entities.Daos;
using APP.Services.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using MySqlConnector;
using System.Data;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddSingleton<IDbConnection>(sp =>
{
    return new MySqlConnection(builder.Configuration.GetConnectionString("MySqlConnection"));

});

builder.Services.AddSingleton<IDAOUsuario, DAOUsuario>();

builder.Services.AddSingleton<IUsuarioService, UsuarioService>();

builder.Services.AddSingleton<IDAOLibro, DAOLibro>();

builder.Services.AddSingleton<ILibroService, LibroService>();

//Configuro Cors, no olvidar agregar el middleware
builder.Services.AddCors(op =>
{
    op.AddDefaultPolicy(
        policy =>
        {              //Para poder verlos en posman debo agregar en el header origin:localhost
            policy.WithOrigins("*");
        });
});


//Paso 1. Configuracion de la validación por JWT. Autenticacion
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(
    options =>
    {
        //VER para que sirven? 
        //options.RequireHttpsMetadata = false;  Para deshabilitar el requerimiento de Https
        //options.SaveToken = true;              Para guardar el token, a donde?

        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,  //Validar el tiempo de vida del token
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],  //Quien es el que origina el token
            ValidAudience = builder.Configuration["Jwt:Audience"],  //A quien va dirigido el token
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
            //ClockSkew = TimeSpan.Zero, para que no haya ningun tipo de desviacion en el reloj en cuanto al nivel de vida del token 
        };
    });

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors();
//Paso 2. Declarar que vamos a utilizar el middleware de autorización y autenticación de .net
app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();


//-https://www.youtube.com/watch?v=Yw9s-Hrds_0