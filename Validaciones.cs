using System;
using System.Globalization;

namespace MesaPartes
{
    /// <summary>
    /// Validación de datos con cadenas de caracteres y estructuras condicionales.
    /// </summary>
    public static class Validaciones
    {
        /// <summary>Devuelve true si la cadena tiene solo dígitos del 0 al 9.</summary>
        private static bool SoloDigitos(string s)
        {
            if (s.Length == 0) return false;
            foreach (char c in s)
            {
                if (c < '0' || c > '9') return false;
            }
            return true;
        }

        /// <summary>El DNI debe tener exactamente 8 dígitos.</summary>
        public static bool ValidarDni(string dni)
        {
            if (dni == null) return false;
            dni = dni.Trim();
            return dni.Length == 8 && SoloDigitos(dni);
        }

        /// <summary>El nombre debe tener al menos 2 palabras formadas solo por letras.</summary>
        public static bool ValidarNombre(string nombre)
        {
            if (nombre == null) return false;
            string[] palabras = nombre.Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries);
            if (palabras.Length < 2) return false;
            foreach (string palabra in palabras)
            {
                foreach (char c in palabra)
                {
                    if (!char.IsLetter(c)) return false;
                }
            }
            return true;
        }

        /// <summary>El celular debe tener 9 dígitos y empezar en 9.</summary>
        public static bool ValidarTelefono(string telefono)
        {
            if (telefono == null) return false;
            telefono = telefono.Trim();
            return telefono.Length == 9 && SoloDigitos(telefono) && telefono[0] == '9';
        }

        /// <summary>El tipo de trámite es una opción del 1 al 4.</summary>
        public static bool ValidarTipo(int tipo)
        {
            return tipo >= 1 && tipo <= 4;
        }

        /// <summary>La fecha debe tener el formato dd/mm/aaaa y ser una fecha real.</summary>
        public static bool ValidarFecha(string fecha)
        {
            if (fecha == null) return false;
            DateTime resultado;
            return DateTime.TryParseExact(fecha.Trim(), "dd/MM/yyyy",
                CultureInfo.InvariantCulture, DateTimeStyles.None, out resultado);
        }
    }
}
