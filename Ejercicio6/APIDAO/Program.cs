using APP.Entities.DAOs;
using APP.Service.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using MySqlConnector;
using System.Data;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.


//Configuracion de la inyección de dependencias para que el DAO pueda recibir la cadena de conexión desde el appsettings.json.
builder.Services.AddSingleton<IDbConnection>(sp =>
{
    var connectionString = builder.Configuration.GetConnectionString("MySqlConnection");
    var cone = new MySqlConnection(connectionString);
    return cone;  //Influye usar el using? Me tira un error cuando intento acceder a un metodo. NO USAR USING
    //return new MySqlConnection(connectionString);

});

//Aqui cambio AddScoped por AddSingleton
builder.Services.AddSingleton<IDAOUser, DAOUser>();

//Configuracion. Inyeccion de dependecia para los servicios

builder.Services.AddSingleton<IUserService, UserService>();

//Ver la forma de llevar esta variable y configurarla en el servicio. OKA
//var jwtKey = builder.Configuration["Jwt:Key"];


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Paso 1. Configuracion de la validación por JWT. Autenticacion
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(
    options =>
    {
        //VER para que sirven? 
        //options.RequireHttpsMetadata = false;
        //options.SaveToken = true;

        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],  //Quien es el que origina el token
            ValidAudience = builder.Configuration["Jwt:Audience"],  //A quien va dirigido el token
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
        };
    });


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

//Paso 2. Declarar que vamos a utilizar el middleware de autorización y autenticación de .net

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();


