using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MesaPartes
{
    /// <summary>Persistencia en archivo de texto: un expediente por línea, campos separados por "|".</summary>
    public static class Archivos
    {
        public static void Guardar(string ruta)
        {
            List<string> lineas = new List<string>();
            for (int i = 0; i < Almacen.Total; i++)
            {
                lineas.Add(string.Join("|", new string[]
                {
                    Almacen.Codigos[i], Almacen.Dnis[i], Almacen.Nombres[i], Almacen.Telefonos[i],
                    Almacen.Tipos[i], Almacen.Fechas[i], Almacen.Estados[i]
                }));
            }
            File.WriteAllLines(ruta, lineas, new UTF8Encoding(false));
        }

        /// <summary>
        /// Carga los expedientes del archivo (si existe). Devuelve cuántas líneas dañadas se omitieron.
        /// </summary>
        public static int Cargar(string ruta)
        {
            Almacen.Limpiar();
            int omitidas = 0;
            if (!File.Exists(ruta)) return 0;

            foreach (string linea in File.ReadAllLines(ruta, Encoding.UTF8))
            {
                if (linea.Trim().Length == 0) continue;
                string[] campos = linea.Split('|');
                if (campos.Length != 7)
                {
                    omitidas++;
                    continue;
                }
                Almacen.Agregar(campos[0], campos[1], campos[2], campos[3], campos[4], campos[5], campos[6]);
            }
            return omitidas;
        }
    }
}
