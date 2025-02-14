namespace Entidades.DTOs
{
    public class PartidaDTO
    {
        public int PartidaID { get; set; }
        public DateTime FyHInicioP { get; set; }
        public DateTime FyHFinP { get; set; }
        public string? Ronda { get; set; }
        public int JugadorDerrotado { get; set; }
        public int JugadorVencedor { get; set; }

    }
}
