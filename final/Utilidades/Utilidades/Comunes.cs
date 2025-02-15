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
    }
}
