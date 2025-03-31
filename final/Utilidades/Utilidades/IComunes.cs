using Entidades.DTOs;

namespace Utilidades.Utilidades
{
    public interface IComunes
    {
        public string EncriptarSHA256(string texto);
        public string GenerarJWT(UsuarioDTO usuario);
        public bool EsPotenciaDeDos(int n);
        public int CalcularPotenciaDeDos(int inscriptos, out int sobrante);
        public int CalcularCantidadPartidas(DateTime inicio, DateTime fin);
        public void CalcularMaximos(int diasDuracion, int juegosPorDia, out int juegosTotales, out int maxJugadores);

    }
}