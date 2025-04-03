using AccesoDatos.DAOs.Acceso;
using AccesoDatos.DAOs.Administrador;
using AccesoDatos.DAOs.Juez;
using AccesoDatos.DAOs.Jugador;
using AccesoDatos.DAOs.Organizador;
using Entidades.DTOs;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Data.SqlClient;
using Microsoft.IdentityModel.Tokens;
using Servicios.Servicios.Acceso;
using Servicios.Servicios.Administrador;
using Servicios.Servicios.Juez;
using Servicios.Servicios.Jugador;
using Servicios.Servicios.Organizador;
using System.Data;
using System.Text;
using Utilidades.Utilidades;
using Validaciones.Middlewares;
using Validaciones.Validacion;



namespace Juego
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.


            //Agregar la cadena de conexion al servicio
            builder.Services.AddScoped<IDbConnection>(sp =>
            {
                var conec = new SqlConnection(builder.Configuration.GetConnectionString("SqlConnection"));
                conec.Open();
                return conec;
            });

            //Agrego utilidades al proyecto
            builder.Services.AddScoped<IComunes, Comunes>();


            //Inyeccion de dependecia para el Controlador de Acceso
            builder.Services.AddScoped<IDAOAcceso, DAOAcceso>();
            builder.Services.AddScoped<IAccesoServicio, AccesoServicio>();

            //Para actuar como un cliente al consumir alguna api, revisar url siempre
            builder.Services.AddHttpClient<IAccesoServicio, AccesoServicio>( c => {
                c.BaseAddress = new Uri(builder.Configuration["BaseUrlPosts"]!);
            });

            //Inyeccion de dependecia para el Controlador de Administradores
            builder.Services.AddScoped<IDAOAdministrador, DAOAdministrador>();
            builder.Services.AddScoped<IAdministradorServicio, AdministradorServicio>();

            //Inyeccion de dependecia para el Controlador de Organizadores
            builder.Services.AddScoped<IDAOOrganizador, DAOOrganizador>();
            builder.Services.AddScoped<IOrganizadorServicio, OrganizadorServicio>();

            //Inyeccion de dependecia para el Controlador de Jueces
            builder.Services.AddScoped<IDAOJuez, DAOJuez>();
            builder.Services.AddScoped<IJuezServicio, JuezServicio>();

            //Inyeccion de dependecia para el Controlador de Jugadores
            builder.Services.AddScoped<IDAOJugador, DAOJugador>();
            builder.Services.AddScoped<IJugadorServicio, JugadorServicio>();

            //Inyeccion de depencia para las validaciones, falta la otra parte - SACAR(es para fluent validation)
            //builder.Services.AddScoped<IValidator<UsuarioDTO>,ValidadorUsuario>();

            //builder.Services.AddScoped<GlobalExceptionHandlingMiddleware>();

            //Haciendo una prueba de Obtener el Id del Usuario Jugador cuando ya hizo el logeo
            builder.Services.AddHttpContextAccessor();



            //Agregando autenticacion y autorizacion
            builder.Services.AddAuthentication(config => {
                config.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                config.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(config =>
                {
                    config.RequireHttpsMetadata = true;
                    config.SaveToken = true;
                    config.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.Zero,
                        IssuerSigningKey = new SymmetricSecurityKey
                        (Encoding.UTF8.GetBytes(builder.Configuration["Jwt:key"]!))
                    };
                }
            );

            //builder.Services.AddAuthorization(options =>
            //{
            //    options.AddPolicy("Administrador", policy => policy.RequireRole("Administrador"));
            //});


            //app.UseMiddleware<ExceptionMiddleware>(); 

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

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseMiddleware<GlobalExceptionHandlingMiddleware>();

            app.MapControllers();

            app.Run();

        }
    }
}
