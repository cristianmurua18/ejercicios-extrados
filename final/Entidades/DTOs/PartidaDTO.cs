namespace Entidades.DTOs
{
    public class PartidaDTO
    {
        public int PartidaID { get; set; }
        public DateTime FyHInicio { get; set; }
        public DateTime FyHFin { get; set; }
        public int HsDiarias { get; set; }
        public string? Ronda { get; set; }
        public int JugadorUno { get; set; }
        public int JugadorDos { get; set; }
        public int JugadorVencedor { get; set; }

    }
}
