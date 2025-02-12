using Entidades.DTOs;

namespace Utilidades.Utilidades
{
    public interface IComunes
    {
        public string EncriptarSHA256(string texto);
        public string GenerarJWT(UsuarioDTO usuario);

    }
}