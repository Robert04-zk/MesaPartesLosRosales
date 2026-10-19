using System;
using System.Collections.Generic;

namespace MesaPartes
{
    /// <summary>Algoritmos reutilizables de búsqueda y ordenamiento sobre arreglos.</summary>
    public static class BusquedaOrden
    {
        /// <summary>Búsqueda lineal: devuelve la posición o -1.</summary>
        public static int BuscarLineal(List<string> arreglo, string valor)
        {
            for (int i = 0; i < arreglo.Count; i++)
            {
                if (arreglo[i] == valor) return i;
            }
            return -1;
        }

        /// <summary>Búsqueda binaria: el arreglo debe estar ordenado de forma ascendente.</summary>
        public static int BuscarBinaria(List<string> arregloOrdenado, string valor)
        {
            int inicio = 0;
            int fin = arregloOrdenado.Count - 1;
            while (inicio <= fin)
            {
                int medio = (inicio + fin) / 2;
                int comparacion = string.CompareOrdinal(arregloOrdenado[medio], valor);
                if (comparacion == 0) return medio;
                if (comparacion < 0) inicio = medio + 1;
                else fin = medio - 1;
            }
            return -1;
        }

        /// <summary>
        /// Ordenamiento por selección sobre la lista "clave". Al intercambiar posiciones
        /// se mueven TODOS los arreglos paralelos para no perder la relación entre los datos.
        /// </summary>
        public static void OrdenarPor(List<string> clave, bool porCodigo)
        {
            int n = clave.Count;
            for (int i = 0; i < n - 1; i++)
            {
                int menor = i;
                for (int j = i + 1; j < n; j++)
                {
                    if (Comparar(clave[j], clave[menor], porCodigo) < 0) menor = j;
                }
                if (menor != i) Almacen.Intercambiar(i, menor);
            }
        }

        private static int Comparar(string a, string b, bool porCodigo)
        {
            if (porCodigo) return string.CompareOrdinal(a, b);
            return string.Compare(a, b, StringComparison.InvariantCultureIgnoreCase);
        }
    }
}
