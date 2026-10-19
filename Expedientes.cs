using System;
using System.Collections.Generic;

namespace MesaPartes
{
    /// <summary>
    /// Arreglos paralelos: la posición i de todas las listas describe un mismo expediente.
    /// </summary>
    public static class Almacen
    {
        public static List<string> Codigos = new List<string>();
        public static List<string> Dnis = new List<string>();
        public static List<string> Nombres = new List<string>();
        public static List<string> Telefonos = new List<string>();
        public static List<string> Tipos = new List<string>();
        public static List<string> Fechas = new List<string>();
        public static List<string> Estados = new List<string>();

        public static int Total
        {
            get { return Codigos.Count; }
        }

        public static void Limpiar()
        {
            Codigos.Clear(); Dnis.Clear(); Nombres.Clear(); Telefonos.Clear();
            Tipos.Clear(); Fechas.Clear(); Estados.Clear();
        }

        public static void Agregar(string codigo, string dni, string nombre, string telefono,
                                   string tipo, string fecha, string estado)
        {
            Codigos.Add(codigo); Dnis.Add(dni); Nombres.Add(nombre); Telefonos.Add(telefono);
            Tipos.Add(tipo); Fechas.Add(fecha); Estados.Add(estado);
        }

        /// <summary>Intercambia las posiciones i y j en TODOS los arreglos a la vez.</summary>
        public static void Intercambiar(int i, int j)
        {
            Swap(Codigos, i, j); Swap(Dnis, i, j); Swap(Nombres, i, j); Swap(Telefonos, i, j);
            Swap(Tipos, i, j); Swap(Fechas, i, j); Swap(Estados, i, j);
        }

        private static void Swap(List<string> lista, int i, int j)
        {
            string temporal = lista[i];
            lista[i] = lista[j];
            lista[j] = temporal;
        }
    }

    /// <summary>Lógica de la mesa de partes: registrar, consultar y listar.</summary>
    public static class Expedientes
    {
        public static readonly string[] NombresTramite =
        {
            "Historia clinica nueva",
            "Solicitud de certificado",
            "Entrega de resultados",
            "Referencia"
        };

        /// <summary>Genera el código correlativo EXP-AAAA-####.</summary>
        public static string GenerarCodigo(string fecha)
        {
            string anio = fecha.Trim().Substring(6, 4);
            return "EXP-" + anio + "-" + (Almacen.Total + 1).ToString().PadLeft(4, '0');
        }

        /// <summary>
        /// Valida y registra un expediente. Devuelve si fue exitoso y un mensaje
        /// (el código generado o el motivo del rechazo).
        /// </summary>
        public static (bool Exito, string Mensaje) Registrar(string dni, string nombre,
            string telefono, int tipo, string fecha)
        {
            if (!Validaciones.ValidarDni(dni)) return (false, "DNI invalido: debe tener 8 digitos.");
            if (!Validaciones.ValidarNombre(nombre)) return (false, "Nombre invalido: minimo 2 palabras, solo letras.");
            if (!Validaciones.ValidarTelefono(telefono)) return (false, "Celular invalido: 9 digitos y empieza en 9.");
            if (!Validaciones.ValidarTipo(tipo)) return (false, "Tipo de tramite invalido: elija del 1 al 4.");
            if (!Validaciones.ValidarFecha(fecha)) return (false, "Fecha invalida: use dd/mm/aaaa.");

            dni = dni.Trim();
            string nombreTipo = NombresTramite[tipo - 1];
            fecha = fecha.Trim();

            // Detección de duplicados: mismo DNI, mismo trámite y misma fecha.
            for (int i = 0; i < Almacen.Total; i++)
            {
                if (Almacen.Dnis[i] == dni && Almacen.Tipos[i] == nombreTipo && Almacen.Fechas[i] == fecha)
                {
                    return (false, "Expediente duplicado: ya existe el mismo DNI, tramite y fecha.");
                }
            }

            string codigo = GenerarCodigo(fecha);
            Almacen.Agregar(codigo, dni, string.Join(" ", nombre.Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries)),
                telefono.Trim(), nombreTipo, fecha, "Recibido");
            return (true, codigo);
        }

        /// <summary>Texto de una ficha de expediente (posición i).</summary>
        public static string Formatear(int i)
        {
            return Almacen.Codigos[i] + " | " + Almacen.Dnis[i] + " | " + Almacen.Nombres[i] + " | "
                 + Almacen.Telefonos[i] + " | " + Almacen.Tipos[i] + " | " + Almacen.Fechas[i]
                 + " | " + Almacen.Estados[i];
        }

        /// <summary>Posición del expediente con ese código, o -1.</summary>
        public static int ConsultarPorCodigo(string codigo)
        {
            return BusquedaOrden.BuscarLineal(Almacen.Codigos, codigo.Trim().ToUpper());
        }

        /// <summary>Posición del primer expediente con ese DNI, o -1.</summary>
        public static int ConsultarPorDni(string dni)
        {
            return BusquedaOrden.BuscarLineal(Almacen.Dnis, dni.Trim());
        }

        /// <summary>Ordena todos los expedientes por código (1) o por nombre (2).</summary>
        public static void Ordenar(int criterio)
        {
            if (criterio == 1) BusquedaOrden.OrdenarPor(Almacen.Codigos, true);
            else BusquedaOrden.OrdenarPor(Almacen.Nombres, false);
        }
    }
}
