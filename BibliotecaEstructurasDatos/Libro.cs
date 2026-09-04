namespace BibliotecaEstructurasDatos
{
    public class Libro
    {
        public string ISBN { get; set; }
        public string Titulo { get; set; }
        public string Autor { get; set; }
        public string Categoria { get; set; }
        public int Anio { get; set; }

        public Libro(string isbn, string titulo, string autor, string categoria, int anio)
        {
            ISBN = isbn;
            Titulo = titulo;
            Autor = autor;
            Categoria = categoria;
            Anio = anio;
        }

        public override string ToString()
        {
            return $"ISBN: {ISBN}\n" +
                   $"Título: {Titulo}\n" +
                   $"Autor: {Autor}\n" +
                   $"Categoría: {Categoria}\n" +
                   $"Año: {Anio}";
        }
    }
}