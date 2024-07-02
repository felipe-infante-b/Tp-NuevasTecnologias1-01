namespace Biblioteca07.Models
{
    public class Alquiler
    {
        public int Id { get; set; }
        public int LibroId { get; set; }
        public Libro? Libro { get; set; }
        public int ClienteId { get; set; }
        public Cliente? Cliente { get; set; }

        public String? Devuelto { get; set; }
    }
}
