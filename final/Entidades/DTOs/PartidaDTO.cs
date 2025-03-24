namespace Entidades.DTOs
{
    public class PartidaDTO
    {
        public int PartidaID { get; set; }
        public int IdRonda { get; set; }
        public int NumeroPartida { get; set; }
        public DateTime FyHInicioP { get; set; }
        public DateTime FyHFinP { get; set; }
        public int JugadorUno { get; set; }
        public int JugadorDos { get; set; }
        public int Ganador { get; set; }
    }
}
