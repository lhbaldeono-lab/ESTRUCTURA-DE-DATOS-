using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace BibliotecaEstructurasDatos
{
    internal class Program
    {
        // CONJUNTO: categorías únicas
        static HashSet<string> categorias = new HashSet<string>();

        // MAPA: ISBN relacionado con cada libro
        static SortedDictionary<string, Libro> librosPorISBN =
            new SortedDictionary<string, Libro>();

        // DICCIONARIO: autor relacionado con sus libros
        static Dictionary<string, List<string>> librosPorAutor =
            new Dictionary<string, List<string>>();

        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            int opcion;

            do
            {
                MostrarMenu();

                Console.Write("Seleccione una opción: ");

                if (!int.TryParse(Console.ReadLine(), out opcion))
                {
                    opcion = 0;
                }

                Console.Clear();

                switch (opcion)
                {
                    case 1:
                        RegistrarLibro();
                        break;

                    case 2:
                        MostrarTodosLosLibros();
                        break;

                    case 3:
                        BuscarLibroPorISBN();
                        break;

                    case 4:
                        BuscarLibrosPorAutor();
                        break;

                    case 5:
                        MostrarCategorias();
                        break;

                    case 6:
                        EliminarLibro();
                        break;

                    case 7:
                        MostrarReporteGeneral();
                        break;

                    case 8:
                        AnalizarTiempoEjecucion();
                        break;

                    case 9:
                        Console.WriteLine("Programa finalizado correctamente.");
                        break;

                    default:
                        Console.WriteLine("Opción incorrecta. Intente nuevamente.");
                        break;
                }

                if (opcion != 9)
                {
                    Console.WriteLine();
                    Console.WriteLine("Presione una tecla para continuar...");
                    Console.ReadKey();
                    Console.Clear();
                }

            } while (opcion != 9);
        }

        static void MostrarMenu()
        {
            Console.WriteLine("==============================================");
            Console.WriteLine("      SISTEMA DE REGISTRO DE BIBLIOTECA");
            Console.WriteLine("==============================================");
            Console.WriteLine("1. Registrar libro");
            Console.WriteLine("2. Mostrar todos los libros");
            Console.WriteLine("3. Buscar libro por ISBN");
            Console.WriteLine("4. Buscar libros por autor");
            Console.WriteLine("5. Mostrar categorías");
            Console.WriteLine("6. Eliminar libro");
            Console.WriteLine("7. Mostrar reporte general");
            Console.WriteLine("8. Analizar tiempo de ejecución");
            Console.WriteLine("9. Salir");
            Console.WriteLine("==============================================");
        }

        static void RegistrarLibro()
        {
            Console.WriteLine("==============================================");
            Console.WriteLine("              REGISTRO DE LIBRO");
            Console.WriteLine("==============================================");

            Console.Write("Ingrese ISBN: ");
            string isbn = Console.ReadLine() ?? "";

            if (librosPorISBN.ContainsKey(isbn))
            {
                Console.WriteLine();
                Console.WriteLine("ERROR: Ya existe un libro registrado con ese ISBN.");
                return;
            }

            Console.Write("Ingrese título: ");
            string titulo = Console.ReadLine() ?? "";

            Console.Write("Ingrese autor: ");
            string autor = Console.ReadLine() ?? "";

            Console.Write("Ingrese categoría: ");
            string categoria = Console.ReadLine() ?? "";

            Console.Write("Ingrese año de publicación: ");

            if (!int.TryParse(Console.ReadLine(), out int anio))
            {
                Console.WriteLine("Año incorrecto.");
                return;
            }

            Libro nuevoLibro =
                new Libro(isbn, titulo, autor, categoria, anio);

            librosPorISBN.Add(isbn, nuevoLibro);

            categorias.Add(categoria);

            if (!librosPorAutor.ContainsKey(autor))
            {
                librosPorAutor[autor] = new List<string>();
            }

            librosPorAutor[autor].Add(titulo);

            Console.WriteLine();
            Console.WriteLine("Libro registrado correctamente.");
        }

        static void MostrarTodosLosLibros()
        {
            Console.WriteLine("==============================================");
            Console.WriteLine("          LISTADO GENERAL DE LIBROS");
            Console.WriteLine("==============================================");

            if (librosPorISBN.Count == 0)
            {
                Console.WriteLine("No existen libros registrados.");
                return;
            }

            foreach (var elemento in librosPorISBN)
            {
                Console.WriteLine(elemento.Value);
                Console.WriteLine("----------------------------------------------");
            }

            Console.WriteLine($"Total de libros: {librosPorISBN.Count}");
        }

        static void BuscarLibroPorISBN()
        {
            Console.WriteLine("==============================================");
            Console.WriteLine("             BÚSQUEDA POR ISBN");
            Console.WriteLine("==============================================");

            Console.Write("Ingrese el ISBN que desea buscar: ");
            string isbn = Console.ReadLine() ?? "";

            if (librosPorISBN.TryGetValue(isbn, out Libro? libro))
            {
                Console.WriteLine();
                Console.WriteLine("Libro encontrado:");
                Console.WriteLine("----------------------------------------------");
                Console.WriteLine(libro);
            }
            else
            {
                Console.WriteLine();
                Console.WriteLine("No se encontró un libro con ese ISBN.");
            }
        }

        static void BuscarLibrosPorAutor()
        {
            Console.WriteLine("==============================================");
            Console.WriteLine("             BÚSQUEDA POR AUTOR");
            Console.WriteLine("==============================================");

            Console.Write("Ingrese el nombre del autor: ");
            string autor = Console.ReadLine() ?? "";

            if (librosPorAutor.TryGetValue(autor, out List<string>? libros))
            {
                Console.WriteLine();
                Console.WriteLine($"Libros registrados de {autor}:");

                foreach (string titulo in libros)
                {
                    Console.WriteLine($"- {titulo}");
                }
            }
            else
            {
                Console.WriteLine();
                Console.WriteLine("No existen libros registrados para ese autor.");
            }
        }

        static void MostrarCategorias()
        {
            Console.WriteLine("==============================================");
            Console.WriteLine("          CATEGORÍAS REGISTRADAS");
            Console.WriteLine("==============================================");

            if (categorias.Count == 0)
            {
                Console.WriteLine("No existen categorías registradas.");
                return;
            }

            foreach (string categoria in categorias.OrderBy(c => c))
            {
                Console.WriteLine($"- {categoria}");
            }

            Console.WriteLine();
            Console.WriteLine($"Total de categorías únicas: {categorias.Count}");
        }

        static void EliminarLibro()
        {
            Console.WriteLine("==============================================");
            Console.WriteLine("               ELIMINAR LIBRO");
            Console.WriteLine("==============================================");

            Console.Write("Ingrese el ISBN del libro que desea eliminar: ");
            string isbn = Console.ReadLine() ?? "";

            if (!librosPorISBN.TryGetValue(isbn, out Libro? libro))
            {
                Console.WriteLine();
                Console.WriteLine("No existe un libro con ese ISBN.");
                return;
            }

            librosPorISBN.Remove(isbn);

            if (librosPorAutor.ContainsKey(libro.Autor))
            {
                librosPorAutor[libro.Autor].Remove(libro.Titulo);

                if (librosPorAutor[libro.Autor].Count == 0)
                {
                    librosPorAutor.Remove(libro.Autor);
                }
            }

            bool categoriaTodaviaExiste =
                librosPorISBN.Values.Any(
                    l => l.Categoria.Equals(
                        libro.Categoria,
                        StringComparison.OrdinalIgnoreCase));

            if (!categoriaTodaviaExiste)
            {
                categorias.Remove(libro.Categoria);
            }

            Console.WriteLine();
            Console.WriteLine($"Libro '{libro.Titulo}' eliminado correctamente.");
        }

        static void MostrarReporteGeneral()
        {
            Console.WriteLine("==============================================");
            Console.WriteLine("              REPORTE GENERAL");
            Console.WriteLine("==============================================");

            Console.WriteLine($"Total de libros registrados: {librosPorISBN.Count}");
            Console.WriteLine($"Total de autores registrados: {librosPorAutor.Count}");
            Console.WriteLine($"Total de categorías únicas: {categorias.Count}");

            Console.WriteLine();
            Console.WriteLine("CATEGORÍAS:");

            foreach (string categoria in categorias.OrderBy(c => c))
            {
                Console.WriteLine($"- {categoria}");
            }

            Console.WriteLine();
            Console.WriteLine("AUTORES Y LIBROS:");

            foreach (var autor in librosPorAutor)
            {
                Console.WriteLine();
                Console.WriteLine($"Autor: {autor.Key}");

                foreach (string titulo in autor.Value)
                {
                    Console.WriteLine($"   - {titulo}");
                }
            }

            Console.WriteLine();
            Console.WriteLine("LIBROS:");

            foreach (var libro in librosPorISBN)
            {
                Console.WriteLine("----------------------------------------------");
                Console.WriteLine(libro.Value);
            }
        }

        static void AnalizarTiempoEjecucion()
        {
            Console.WriteLine("==============================================");
            Console.WriteLine("        ANÁLISIS DE TIEMPO DE EJECUCIÓN");
            Console.WriteLine("==============================================");

            if (librosPorISBN.Count == 0)
            {
                Console.WriteLine("Primero debe registrar al menos un libro.");
                return;
            }

            Console.Write("Ingrese un ISBN para realizar la prueba: ");
            string isbn = Console.ReadLine() ?? "";

            Stopwatch reloj = new Stopwatch();

            reloj.Start();

            bool encontrado = librosPorISBN.ContainsKey(isbn);

            reloj.Stop();

            Console.WriteLine();
            Console.WriteLine("Resultado de la búsqueda:");

            if (encontrado)
            {
                Console.WriteLine("Libro encontrado.");
            }
            else
            {
                Console.WriteLine("Libro no encontrado.");
            }

            Console.WriteLine();
            Console.WriteLine(
                $"Tiempo transcurrido: {reloj.Elapsed.TotalMilliseconds:F6} ms");

            Console.WriteLine(
                $"Ticks utilizados: {reloj.ElapsedTicks}");
        }
    }
}