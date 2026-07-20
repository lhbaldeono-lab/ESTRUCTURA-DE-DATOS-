using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

// Esta clase guarda la información de cada visitante.
public class Visitante
{
    public int Id { get; }

    public string Nombre { get; }

    public DateTime HoraLlegada { get; }

    public int? NumeroAsiento { get; private set; }

    public Visitante(int id, string nombre)
    {
        Id = id;
        Nombre = nombre;
        HoraLlegada = DateTime.Now;
    }

    public void AsignarAsiento(int numero)
    {
        NumeroAsiento = numero;
    }
}

// Esta clase representa cada uno de los 30 asientos.
public class Asiento
{
    public int Numero { get; }

    public Visitante? VisitanteAsignado { get; private set; }

    public bool Ocupado
    {
        get
        {
            return VisitanteAsignado != null;
        }
    }

    public Asiento(int numero)
    {
        Numero = numero;
    }

    public void Asignar(Visitante visitante)
    {
        VisitanteAsignado = visitante;

        visitante.AsignarAsiento(Numero);
    }

    public void Liberar()
    {
        VisitanteAsignado = null;
    }
}

// Esta clase controla la cola y los asientos.
public class SistemaAtraccion
{
    public const int CapacidadMaxima = 30;

    private readonly Queue<Visitante> cola;

    private readonly List<Asiento> asientos;

    private int siguienteId;

    public SistemaAtraccion()
    {
        cola = new Queue<Visitante>();

        asientos = new List<Asiento>();

        siguienteId = 1;

        for (int numero = 1;
             numero <= CapacidadMaxima;
             numero++)
        {
            asientos.Add(new Asiento(numero));
        }
    }

    public int CantidadEnCola
    {
        get
        {
            return cola.Count;
        }
    }

    public int AsientosOcupados
    {
        get
        {
            return asientos.Count(
                asiento => asiento.Ocupado);
        }
    }

    public int AsientosLibres
    {
        get
        {
            return CapacidadMaxima -
                   AsientosOcupados;
        }
    }

    public int EntradasVendidas
    {
        get
        {
            return CantidadEnCola +
                   AsientosOcupados;
        }
    }

    public int CuposDisponibles
    {
        get
        {
            return CapacidadMaxima -
                   EntradasVendidas;
        }
    }

    // Registra a una persona al final de la cola.
    public string RegistrarVisitante(string nombre)
    {
        if (string.IsNullOrWhiteSpace(nombre))
        {
            return "ERROR: debe escribir un nombre.";
        }

        if (EntradasVendidas >= CapacidadMaxima)
        {
            return
                "REGISTRO RECHAZADO: no existen cupos disponibles. " +
                "Las 30 entradas fueron vendidas.";
        }

        Visitante visitante =
            new Visitante(
                siguienteId,
                nombre.Trim());

        siguienteId++;

        cola.Enqueue(visitante);

        return
            $"{visitante.Nombre} fue registrado correctamente.\n" +
            $"Identificación: {visitante.Id}\n" +
            $"Posición en la cola: {cola.Count}";
    }

    // Atiende al primero que llegó y le asigna un asiento.
    public string AsignarSiguienteAsiento()
    {
        if (cola.Count == 0)
        {
            return
                "No hay visitantes esperando en la cola.";
        }

        Asiento? asientoLibre =
            asientos.FirstOrDefault(
                asiento => !asiento.Ocupado);

        if (asientoLibre == null)
        {
            return
                "No existen asientos disponibles.";
        }

        Visitante primeroEnLaCola =
            cola.Dequeue();

        asientoLibre.Asignar(
            primeroEnLaCola);

        return
            $"Asiento {asientoLibre.Numero:00} asignado a " +
            $"{primeroEnLaCola.Nombre}.\n" +
            "La atención respetó el orden FIFO.";
    }

    // Muestra las personas que todavía están esperando.
    public void MostrarCola()
    {
        Console.WriteLine(
            "============================================================");

        Console.WriteLine(
            "              VISITANTES EN LA COLA");

        Console.WriteLine(
            "============================================================");

        if (cola.Count == 0)
        {
            Console.WriteLine(
                "La cola está vacía.");

            return;
        }

        Console.WriteLine(
            "{0,-10} {1,-6} {2,-25} {3,-12}",
            "Posición",
            "ID",
            "Nombre",
            "Llegada");

        Console.WriteLine(
            new string('-', 60));

        int posicion = 1;

        foreach (Visitante visitante in cola)
        {
            Console.WriteLine(
                "{0,-10} {1,-6} {2,-25} {3,-12}",
                posicion,
                visitante.Id,
                visitante.Nombre,
                visitante.HoraLlegada.ToString("HH:mm:ss"));

            posicion++;
        }

        Console.WriteLine(
            new string('-', 60));

        Console.WriteLine(
            $"Total de personas en espera: {cola.Count}");
    }

    // Muestra el estado de los 30 asientos.
    public void MostrarAsientos()
    {
        Console.WriteLine(
            "============================================================");

        Console.WriteLine(
            "                 REPORTE DE ASIENTOS");

        Console.WriteLine(
            "============================================================");

        Console.WriteLine(
            "{0,-10} {1,-12} {2,-6} {3,-25}",
            "Asiento",
            "Estado",
            "ID",
            "Visitante");

        Console.WriteLine(
            new string('-', 60));

        foreach (Asiento asiento in asientos)
        {
            if (asiento.Ocupado)
            {
                Console.WriteLine(
                    "{0,-10} {1,-12} {2,-6} {3,-25}",
                    asiento.Numero.ToString("00"),
                    "OCUPADO",
                    asiento.VisitanteAsignado!.Id,
                    asiento.VisitanteAsignado.Nombre);
            }
            else
            {
                Console.WriteLine(
                    "{0,-10} {1,-12} {2,-6} {3,-25}",
                    asiento.Numero.ToString("00"),
                    "LIBRE",
                    "-",
                    "-");
            }
        }

        Console.WriteLine(
            new string('-', 60));

        Console.WriteLine(
            $"Asientos ocupados: {AsientosOcupados}");

        Console.WriteLine(
            $"Asientos libres: {AsientosLibres}");
    }

    // Busca un visitante mediante su nombre o identificación.
    public void BuscarVisitante(string texto)
    {
        if (string.IsNullOrWhiteSpace(texto))
        {
            Console.WriteLine(
                "Debe escribir un nombre o identificación.");

            return;
        }

        List<Visitante> visitantesConAsiento =
            asientos
                .Where(
                    asiento =>
                        asiento.VisitanteAsignado != null)
                .Select(
                    asiento =>
                        asiento.VisitanteAsignado!)
                .ToList();

        List<Visitante> encontrados =
            cola
                .Concat(visitantesConAsiento)
                .Where(
                    visitante =>
                        visitante.Id.ToString() ==
                        texto.Trim() ||

                        visitante.Nombre.Contains(
                            texto.Trim(),
                            StringComparison.OrdinalIgnoreCase))
                .OrderBy(
                    visitante => visitante.Id)
                .ToList();

        Console.WriteLine(
            "=============================================");

        Console.WriteLine(
            "          CONSULTA DE VISITANTE");

        Console.WriteLine(
            "=============================================");

        if (encontrados.Count == 0)
        {
            Console.WriteLine(
                "No se encontró ningún visitante.");

            return;
        }

        foreach (Visitante visitante in encontrados)
        {
            Console.WriteLine(
                $"Identificación: {visitante.Id}");

            Console.WriteLine(
                $"Nombre: {visitante.Nombre}");

            Console.WriteLine(
                $"Hora de llegada: " +
                $"{visitante.HoraLlegada:HH:mm:ss}");

            if (visitante.NumeroAsiento.HasValue)
            {
                Console.WriteLine(
                    $"Estado: asiento " +
                    $"{visitante.NumeroAsiento.Value:00}");
            }
            else
            {
                int posicion =
                    cola
                        .ToList()
                        .FindIndex(
                            persona =>
                                persona.Id ==
                                visitante.Id) + 1;

                Console.WriteLine(
                    $"Estado: esperando en la posición {posicion}");
            }

            Console.WriteLine(
                new string('-', 45));
        }
    }

    // Muestra un resumen de la atracción.
    public void MostrarEstadoGeneral()
    {
        Console.WriteLine(
            "=============================================");

        Console.WriteLine(
            "          ESTADO GENERAL DEL SISTEMA");

        Console.WriteLine(
            "=============================================");

        Console.WriteLine(
            $"Capacidad máxima: {CapacidadMaxima}");

        Console.WriteLine(
            $"Entradas vendidas: {EntradasVendidas}");

        Console.WriteLine(
            $"Cupos disponibles: {CuposDisponibles}");

        Console.WriteLine(
            $"Personas en la cola: {CantidadEnCola}");

        Console.WriteLine(
            $"Asientos ocupados: {AsientosOcupados}");

        Console.WriteLine(
            $"Asientos libres: {AsientosLibres}");

        if (cola.Count > 0)
        {
            Console.WriteLine(
                $"Siguiente visitante: {cola.Peek().Nombre}");
        }
        else
        {
            Console.WriteLine(
                "Siguiente visitante: ninguno");
        }
    }

    // Realiza la prueba de los 30 asientos y mide el tiempo.
    public void EjecutarPruebaAutomatica()
    {
        Reiniciar();

        Stopwatch reloj =
            Stopwatch.StartNew();

        for (int numero = 1;
             numero <= CapacidadMaxima;
             numero++)
        {
            RegistrarVisitante(
                $"Visitante {numero:00}");
        }

        string intentoPersona31 =
            RegistrarVisitante(
                "Visitante 31");

        bool capacidadCorrecta =
            intentoPersona31.Contains(
                "RECHAZADO");

        while (cola.Count > 0)
        {
            AsignarSiguienteAsiento();
        }

        reloj.Stop();

        List<int> ordenObtenido =
            asientos
                .Where(
                    asiento =>
                        asiento.Ocupado)
                .OrderBy(
                    asiento =>
                        asiento.Numero)
                .Select(
                    asiento =>
                        asiento.VisitanteAsignado!.Id)
                .ToList();

        bool fifoCorrecto =
            ordenObtenido.SequenceEqual(
                Enumerable.Range(
                    1,
                    CapacidadMaxima));

        Console.WriteLine(
            "=============================================");

        Console.WriteLine(
            "       RESULTADOS DE LA PRUEBA AUTOMÁTICA");

        Console.WriteLine(
            "=============================================");

        Console.WriteLine(
            $"Visitantes registrados: {EntradasVendidas}");

        Console.WriteLine(
            $"Asientos asignados: {AsientosOcupados}");

        Console.WriteLine(
            "Control de capacidad: " +
            (capacidadCorrecta
                ? "CORRECTO"
                : "REVISAR"));

        Console.WriteLine(
            "Orden de llegada FIFO: " +
            (fifoCorrecto
                ? "CORRECTO"
                : "REVISAR"));

        Console.WriteLine(
            $"Tiempo de ejecución: " +
            $"{reloj.Elapsed.TotalMilliseconds:F4} " +
            "milisegundos");

        Console.WriteLine(
            $"Tics registrados: {reloj.ElapsedTicks}");
    }

    // Elimina todos los registros.
    public void Reiniciar()
    {
        cola.Clear();

        foreach (Asiento asiento in asientos)
        {
            asiento.Liberar();
        }

        siguienteId = 1;
    }
}

// Esta es la clase que inicia el programa.
public class Program
{
    public static void Main()
    {
        Console.OutputEncoding =
            Encoding.UTF8;

        SistemaAtraccion sistema =
            new SistemaAtraccion();

        bool continuar = true;

        while (continuar)
        {
            Console.Clear();

            Console.WriteLine(
                "================================================");

            Console.WriteLine(
                "     SISTEMA DE ASIGNACIÓN DE 30 ASIENTOS");

            Console.WriteLine(
                "               PROGRAMA EN C#");

            Console.WriteLine(
                "================================================");

            Console.WriteLine(
                "1. Registrar visitante");

            Console.WriteLine(
                "2. Asignar siguiente asiento");

            Console.WriteLine(
                "3. Mostrar cola de espera");

            Console.WriteLine(
                "4. Mostrar los 30 asientos");

            Console.WriteLine(
                "5. Buscar visitante");

            Console.WriteLine(
                "6. Mostrar estado general");

            Console.WriteLine(
                "7. Ejecutar prueba automática");

            Console.WriteLine(
                "8. Reiniciar sistema");

            Console.WriteLine(
                "0. Salir");

            Console.WriteLine(
                "================================================");

            Console.Write(
                "Escriba una opción: ");

            string opcion =
                Console.ReadLine() ?? "";

            Console.Clear();

            switch (opcion)
            {
                case "1":

                    Console.WriteLine(
                        "REGISTRO DE VISITANTE");

                    Console.Write(
                        "Escriba el nombre: ");

                    string nombre =
                        Console.ReadLine() ?? "";

                    Console.WriteLine();

                    Console.WriteLine(
                        sistema.RegistrarVisitante(
                            nombre));

                    break;

                case "2":

                    Console.WriteLine(
                        sistema.AsignarSiguienteAsiento());

                    break;

                case "3":

                    sistema.MostrarCola();

                    break;

                case "4":

                    sistema.MostrarAsientos();

                    break;

                case "5":

                    Console.Write(
                        "Escriba el nombre o la identificación: ");

                    string texto =
                        Console.ReadLine() ?? "";

                    Console.WriteLine();

                    sistema.BuscarVisitante(
                        texto);

                    break;

                case "6":

                    sistema.MostrarEstadoGeneral();

                    break;

                case "7":

                    sistema.EjecutarPruebaAutomatica();

                    break;

                case "8":

                    sistema.Reiniciar();

                    Console.WriteLine(
                        "El sistema fue reiniciado correctamente.");

                    break;

                case "0":

                    continuar = false;

                    Console.WriteLine(
                        "El programa finalizó.");

                    break;

                default:

                    Console.WriteLine(
                        "La opción seleccionada no existe.");

                    break;
            }

            if (continuar)
            {
                Console.WriteLine();

                Console.WriteLine(
                    "Presione cualquier tecla para volver al menú.");

                Console.ReadKey(true);
            }
        }
    }
}