using System;
using System.Text;

namespace MesaPartes
{
    public static class Program
    {
        private const string RUTA = "expedientes.txt";

        public static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;

            if (args.Length > 0 && args[0] == "pruebas")
            {
                Pruebas.Ejecutar();
                return;
            }

            int omitidas = Archivos.Cargar(RUTA);
            if (omitidas > 0) Console.WriteLine("Aviso: se omitieron " + omitidas + " linea(s) danadas del archivo.");

            int opcion;
            do
            {
                MostrarMenu();
                opcion = LeerEntero("Elija una opcion: ");
                switch (opcion)
                {
                    case 1: PedirRegistro(); break;
                    case 2: Consultar(true); break;
                    case 3: Consultar(false); break;
                    case 4: Listar(); break;
                    case 5: Ordenar(); break;
                    case 6: Console.WriteLine("Hasta luego."); break;
                    default: Console.WriteLine("Opcion no valida."); break;
                }
            } while (opcion != 6);
        }

        private static void MostrarMenu()
        {
            Console.WriteLine();
            Console.WriteLine("=== MESA DE PARTES DIGITAL - CONSULTORIO LOS ROSALES ===");
            Console.WriteLine("1. Registrar expediente");
            Console.WriteLine("2. Consultar por codigo");
            Console.WriteLine("3. Consultar por DNI");
            Console.WriteLine("4. Listar todos");
            Console.WriteLine("5. Ordenar (codigo o nombre)");
            Console.WriteLine("6. Salir");
        }

        private static string LeerTexto(string mensaje)
        {
            Console.Write(mensaje);
            string texto = Console.ReadLine();
            return texto == null ? "" : texto;
        }

        private static int LeerEntero(string mensaje)
        {
            int valor;
            if (int.TryParse(LeerTexto(mensaje).Trim(), out valor)) return valor;
            return -1;
        }

        private static void PedirRegistro()
        {
            string dni = LeerTexto("DNI (8 digitos): ");
            string nombre = LeerTexto("Apellidos y nombres: ");
            string telefono = LeerTexto("Celular (9 digitos): ");
            Console.WriteLine("Tipo de tramite: 1. Historia clinica nueva  2. Solicitud de certificado  3. Entrega de resultados  4. Referencia");
            int tipo = LeerEntero("Opcion: ");
            string fecha = DateTime.Now.ToString("dd/MM/yyyy");

            var resultado = Expedientes.Registrar(dni, nombre, telefono, tipo, fecha);
            if (resultado.Exito)
            {
                Archivos.Guardar(RUTA);
                Console.WriteLine("Expediente registrado. Codigo asignado: " + resultado.Mensaje);
            }
            else
            {
                Console.WriteLine("ERROR: " + resultado.Mensaje);
            }
        }

        private static void Consultar(bool porCodigo)
        {
            int posicion;
            if (porCodigo) posicion = Expedientes.ConsultarPorCodigo(LeerTexto("Codigo (EXP-AAAA-####): "));
            else posicion = Expedientes.ConsultarPorDni(LeerTexto("DNI: "));

            if (posicion == -1) Console.WriteLine("No se encontro el expediente.");
            else Console.WriteLine(Expedientes.Formatear(posicion));
        }

        private static void Listar()
        {
            if (Almacen.Total == 0)
            {
                Console.WriteLine("No hay expedientes registrados.");
                return;
            }
            for (int i = 0; i < Almacen.Total; i++) Console.WriteLine(Expedientes.Formatear(i));
        }

        private static void Ordenar()
        {
            int criterio = LeerEntero("Ordenar por 1. Codigo  2. Nombre: ");
            if (criterio != 1 && criterio != 2)
            {
                Console.WriteLine("Criterio no valido.");
                return;
            }
            Expedientes.Ordenar(criterio);
            Archivos.Guardar(RUTA);
            Console.WriteLine("Expedientes ordenados.");
            Listar();
        }
    }
}
