using Entidades.DTOs;
using Entidades.Modelos;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Utilidades.Utilidades
{
    public class Comunes(IConfiguration jwtconfig) : IComunes
    {

        private readonly IConfiguration _configJwt = jwtconfig;
        //Para encriptar contraseñas en la BD
        public string EncriptarSHA256(string texto)
        {
            //Metodo para encriptar
            using (SHA256 sha256Hash = SHA256.Create())
            {
                //Computar el hash
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(texto));

                //Converir el array de bytes a string
                var builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();


            }
        }

        public string GenerarJWT(UsuarioDTO usuario)
        {
            var userClaims = new[]
            {
                //VER QUE DEVOLVER Y PARA QUE PUEDE SERVIR
                new Claim("UsuarioID", usuario.UsuarioID.ToString()),
                new Claim(ClaimTypes.NameIdentifier, usuario.NombreApellido!),
                new Claim(ClaimTypes.Email, usuario.Email!),
                new Claim(ClaimTypes.Role, usuario.Rol!),
                new Claim(ClaimTypes.Country, usuario.NombrePais!)
            };

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configJwt["Jwt:key"]!));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);

            //Crear detalle del token
            var jwtConfig = new JwtSecurityToken(
                claims: userClaims,
                expires: DateTime.UtcNow.AddMinutes(10),
                signingCredentials: credentials
                );

            return new JwtSecurityTokenHandler().WriteToken(jwtConfig);
        }

        //calcular diferencia de tiempo
        public int CalcularCantidadPartidas(DateTime inicio, DateTime fin)
        {
            // Calcular la diferencia
            TimeSpan diferencia = fin - inicio;

            // Obtener días y horas
            int dias = diferencia.Days;
            int horas = diferencia.Hours;
            int minutos = diferencia.Minutes;

            // Mostrar resultado
            string dif = $"Diferencia: {dias} días, {horas} horas, {minutos} minutos.";

            //Diferencia: 3 días y 4 horas y 15 minutos.

            //REGLAS: Maximo de 8hs por dia de juegos y 30 minutos de duracion por juego

            //Para calcular cantidad de partidas hago, = 16 partidas en 8 hs como maximo x dias + partidas por hora X / 0.5(dos por hora)
            var partidasDias = 16 * dias;
            var partidasHoras = horas / 0.5;
            var partidasMinutos = minutos >= 30 ? 1 : 0;

            var cantPartidas = partidasDias + partidasHoras + partidasMinutos;

            return Convert.ToInt32(cantPartidas);

        }

        public void CalcularMaximos(int diasDuracion, int juegosPorDia, out int juegosTotales, out int maxJugadores)
        {
            int juegosPosibles = diasDuracion * juegosPorDia;
            juegosTotales = juegosPosibles - 1;
            maxJugadores = (int)Math.Pow(2, Math.Floor(Math.Log2(juegosPosibles + 1)));
        }

        public int CalcularPotenciaDeDos(int numero)
        {   //El numero no puede ser 0 ni negativos   -- El numero debe ser potencia de dos.
            if (numero < 1 || (numero & (numero - 1)) != 0)
                throw new ArgumentException("El número debe ser una potencia de 2 mayor a 0.");
            else
                //Ver implementacion


                return (int)Math.Log2(numero);
        }

        public int ObtenerNumeroRondas(int inscriptos)
        {
            var rondas = CalcularPotenciaDeDos(inscriptos);

            return rondas;
        }


        public string ObtenerNombreRonda(int numeroRonda)
        {
            return numeroRonda switch
            {
                6 => "64avos",   //128 jugadores max
                5 => "32avos",   //64 jugadores max
                4 => "16avos",   //32 Jugadores max
                3 => "8vos",     //16 jugadores max
                2 => "4tos",     //8 jugadores max
                1 => "Semi",     //4 jugadores max
                0 => "Final",    //2 jugadores max
                _ => "Desconocida" //0 jugadores
            };
        }

    }



}
