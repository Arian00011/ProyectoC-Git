using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Net;
using System.Runtime.ConstrainedExecution;
using System.Text.RegularExpressions;
using static System.Runtime.InteropServices.JavaScript.JSType;

public class Habitacion
{
    private int numero; //Representa el número de la habitación. hi Prueba 1 Git
    private string tipo; // Representa el tipo de habitación (puede ser "Doble", "Individual", etc.).
    private decimal precio; //Precio de la habitación.
    private bool disponible; //Un indicador booleano que representa si la habitación está disponible o no.

    //Propiedades de solo lectura (Numero, Tipo, Precio, Disponible):
    //Estas propiedades permiten acceder a los campos privados desde fuera de la clase de manera 
    //controlada y solo de lectura. Esto significa que otros objetos pueden obtener información 
    //sobre la habitación, pero no pueden modificar estos valores directamente.
    public int Numero => numero;
    public string Tipo => tipo;
    public decimal Precio => precio;
    public bool Disponible => disponible;


    //Este constructor se llama cuando se crea una nueva instancia de la clase Habitacion.
    //Toma como argumentos el número, tipo y precio de la habitación, inicializa los campos 
    //correspondientes y establece el estado de disponibilidad en true por defecto.
    public Habitacion(int numero, string tipo, decimal precio)
    {
        this.numero = numero;
        this.tipo = tipo;
        this.precio = precio;
        this.disponible = true;
    }

    //cuando se llama a este método, se marca la habitación como no disponible "false",
    //indicando que ha sido reservada.
    public void Reservar()
    {
        disponible = false;
    }
}
public class Hotel
{
    //    habitaciones: Una lista que almacena objetos de la clase Habitacion, representando las
    //    habitaciones disponibles en el hotel.
    //    reservas: Una lista que almacena objetos de la clase Reserva, representando las reservas
    //    realizadas en el hotel.
    private List<Habitacion> habitaciones;
    private List<Reserva> reservas;

    public Hotel()
    {
        //Inicializa las listas de habitaciones y reservas con algunas habitaciones predeterminadas
        //al crear una instancia de la clase Hotel.En este caso, se crean dos habitaciones
        //con números 101 y 102.
        habitaciones = new List<Habitacion>
        {
            new Habitacion(101, "Doble", 100),
            new Habitacion(102, "Individual", 75),
        };

        reservas = new List<Reserva>();
    }
    //Este metodo muestra por consola la información de las habitaciones que están actualmente disponibles
    //si "habitacion.Disponible" es true en el hotel.
    public void MostrarHabitacionesDisponibles()
    {
        Console.WriteLine("Habitaciones disponibles:");
        //Iterar sobre la colecció de listas habitaciones
        foreach (var habitacion in habitaciones)
        {
            if (habitacion.Disponible)
            {
                Console.WriteLine($"Número: {habitacion.Numero}, Tipo: {habitacion.Tipo}, Precio: {habitacion.Precio:C}", "$");
            }
        }
    }
    //En el metodo "RealizarReserva" coordina el proceso de realizar una reserva.
    //Captura los datos del cliente utilizando el método CapturarDatosCliente().
    //Muestra las habitaciones disponibles utilizando el método
    public void RealizarReserva()
    {
        Cliente cliente = CapturarDatosCliente();

        MostrarHabitacionesDisponibles();
        //se llama el metodo "SeleccionarHabitacionDisponible" lo que se haga en este metodo se guardara
        //en "habitacionSeleccionada"
        Habitacion habitacionSeleccionada = SeleccionarHabitacionDisponible();

        if (habitacionSeleccionada != null)
        {
            //Si se selecciona una habitación disponible, captura las fechas de inicio y fin de la reserva
            //y crea una instancia de la clase Reserva.
            DateTime fechaInicio = CapturarFecha("Fecha de inicio de la reserva (yyyy-MM-dd): ");
            DateTime fechaFin = CapturarFecha("Fecha de fin de la reserva (yyyy-MM-dd): ");

            //Agrega la reserva a la lista de reservas.
            Reserva reserva = new Reserva(cliente, habitacionSeleccionada, fechaInicio, fechaFin);
            reservas.Add(reserva);

            //Marca la habitación seleccionada como no disponible llamando al método Reservar() de la clase Habitacion.
            habitacionSeleccionada.Reservar();
            // Se genera una factura llamando al método GenerarFactura() y muestra los detalles de la factura por consola.
            Factura factura = GenerarFactura(reserva);
            Console.WriteLine("Reserva realizada con éxito. Detalles de la factura:");
            Console.WriteLine($"Monto Total: {factura.MontoTotal:C}");
            Console.WriteLine($"Fecha de Emisión: {factura.FechaEmision}");
        }
        else
        {
            Console.WriteLine("No se pudo realizar la reserva. La habitación seleccionada no está disponible.");
        }
    }
    // Este metodo solicita al usuario ingresar su nombre y correo electrónico y devuelve
    // un objeto de la clase Cliente con esa información.
    private Cliente CapturarDatosCliente()
    {
        Console.Write("Ingrese su nombre: ");
        string nombre = Console.ReadLine();

        Console.Write("Ingrese su correo electrónico: ");
        string correo = Console.ReadLine();

        return new Cliente(nombre, correo);
    }


    //Este metodo solicita al usuario que seleccione el número de la habitación.
    //Utiliza un bucle para validar la entrada del usuario y garantizar que se seleccione
    //un número de habitación válido y disponible.
    //Devuelve la habitación seleccionada si es válida y disponible.
    private Habitacion SeleccionarHabitacionDisponible()
    {
        Console.Write("Seleccione el número de la habitación: ");
        int numeroHabitacion;
        while (!int.TryParse(Console.ReadLine(), out numeroHabitacion) || !EsNumeroHabitacionValido(numeroHabitacion))
        {
            Console.WriteLine("Número de habitación inválido. Inténtelo de nuevo.");
            Console.Write("Seleccione el número de la habitación: ");
        }

        return habitaciones.Find(h => h.Numero == numeroHabitacion && h.Disponible);
    }

    //Verifica si el número de habitación pasado como argumento existe en la lista de habitaciones del hotel.
    private bool EsNumeroHabitacionValido(int numero)
    {
        return habitaciones.Exists(h => h.Numero == numero);
    }
    //Solicita al usuario que ingrese una fecha en el formato "yyyy-MM-dd".
    //Utiliza un bucle While para validar la entrada del usuario y garantizar que se ingrese una fecha válida.
    //Devuelve la fecha ingresada.
    private DateTime CapturarFecha(string mensaje)
    {
        Console.Write(mensaje);
        DateTime fecha;
        while (!DateTime.TryParseExact(Console.ReadLine(), "yyyy-MM-dd", null, System.Globalization.DateTimeStyles.None, out fecha))
        {
            Console.WriteLine("Formato de fecha inválido. Inténtelo de nuevo (yyyy-MM-dd).");
            Console.Write(mensaje);
        }

        return fecha;
    }
    //Calcula el monto total de la factura llamando al método CalcularMontoTotal()
    //y crea una instancia de la clase Factura con ese monto.
    private Factura GenerarFactura(Reserva reserva)
    {
        decimal montoTotal = CalcularMontoTotal(reserva);
        return new Factura(montoTotal);
    }
    //Calcula el monto total de la reserva multiplicando el precio de la habitación por el número de días de la reserva.
    private decimal CalcularMontoTotal(Reserva reserva)
    {
        return reserva.Habitacion.Precio * (decimal)(reserva.FechaFin - reserva.FechaInicio).TotalDays;
    }
}
public class Reserva
{
    //Almacenan información sobre la reserva, incluyendo el cliente, la habitación,
    //las fechas de inicio y fin de la reserva, y un indicador de si la reserva está confirmada o no.
    private Cliente cliente;
    private Habitacion habitacion;
    private DateTime fechaInicio;
    private DateTime fechaFin;
    private bool confirmada;

    //Proporcionan acceso controlado a los campos privados de la reserva.
    public Cliente Cliente => cliente;
    public Habitacion Habitacion => habitacion;
    public DateTime FechaInicio => fechaInicio;
    public DateTime FechaFin => fechaFin;
    public bool Confirmada => confirmada;

    //Inicializa los campos de la reserva al crear una instancia. La reserva se inicia como no confirmada 
    public Reserva(Cliente cliente, Habitacion habitacion, DateTime fechaInicio, DateTime fechaFin)
    {
        this.cliente = cliente;
        this.habitacion = habitacion;
        this.fechaInicio = fechaInicio;
        this.fechaFin = fechaFin;
        this.confirmada = false;
    }

    // Cambia el estado de la reserva a confirmada, estableciendo confirmada en true.
    internal void ConfirmarReserva()
    {
        confirmada = true;
    }
}
public class Cliente
{
    //nombre y correo almacenan información sobre el cliente, incluyendo su nombre y correo electrónico.
    private string nombre;
    private string correo;

    //Nombre y Correo proporcionan acceso controlado a los campos privados del cliente.
    public string Nombre => nombre;
    public string Correo => correo;

    // Inicializa los campos del cliente al crear una instancia.
    public Cliente(string nombre, string correo)
    {
        this.nombre = nombre;
        this.correo = correo;
    }
    //Muestra la información del cliente por consola.Tiene un modificador de acceso protected internal,
    //lo que significa que puede ser accedido desde clases en el mismo ensamblado y también desde clases derivadas.
    protected internal void MostrarInformacion()
    {
        Console.WriteLine($"Nombre: {nombre}");
        Console.WriteLine($"Correo: {correo}");
    }
}
internal class Factura
{
    //montoTotal y fechaEmision almacenan información sobre la factura, incluyendo el monto total y la fecha de emisión.
    private decimal montoTotal;
    private DateTime fechaEmision;
    //Proporcionan acceso controlado a los campos privados de la factura.
    public decimal MontoTotal => montoTotal;
    public DateTime FechaEmision => fechaEmision;
    //Inicializa los campos de la factura al crear una instancia.Tiene un modificador de acceso internal`,
    //lo que significa que solo puede ser instanciada por clases en el mismo ensamblado.
    public Factura(decimal montoTotal)
    {
        this.montoTotal = montoTotal;
        this.fechaEmision = DateTime.Now;
    }
}
//Define la clase principal de la aplicación.
class Program
{
    static void Main()
    {
        Hotel hotel = new Hotel();
        //Utiliza un switch para realizar un llamado de metodo segun la opcion que se escoja.
        while (true)
        {

            Console.WriteLine("----- Menú Principal -----");
            Console.WriteLine("1. Mostrar habitaciones disponibles");
            Console.WriteLine("2. Realizar reserva");
            Console.WriteLine("3. Salir del programa");
            Console.Write("Seleccione una opción: ");

            string opcion = Console.ReadLine();

            switch (opcion)
            {
                case "1":
                    hotel.MostrarHabitacionesDisponibles();
                    break;
                case "2":
                    hotel.RealizarReserva();
                    break;
                case "3":
                    Console.WriteLine("Saliendo del programa. ¡Hasta luego!");
                    return;
                default:
                    Console.WriteLine("Opción no válida. Inténtelo de nuevo..");
                    break;
            }
        }
    }
}
