using System;
using System.IO;

namespace MesaPartes
{
    /// <summary>Casos de prueba automatizados. Se ejecutan con: dotnet run -- pruebas</summary>
    public static class Pruebas
    {
        private static int aprobados = 0;
        private static int total = 0;

        private static void Caso(string id, string descripcion, string esperado, string obtenido)
        {
            total++;
            bool ok = esperado == obtenido;
            if (ok) aprobados++;
            Console.WriteLine(id.PadRight(6) + "| " + descripcion.PadRight(48) + "| esp: " + esperado.PadRight(15)
                + "| obt: " + obtenido.PadRight(15) + "| " + (ok ? "OK" : "FALLO"));
        }

        public static void Ejecutar()
        {
            Console.WriteLine("--- Validaciones ---");
            Caso("CP-01", "DNI valido 45871236", "True", Validaciones.ValidarDni("45871236").ToString());
            Caso("CP-02", "DNI con 7 digitos", "False", Validaciones.ValidarDni("4587123").ToString());
            Caso("CP-03", "DNI con 9 digitos", "False", Validaciones.ValidarDni("458712369").ToString());
            Caso("CP-04", "DNI con letra", "False", Validaciones.ValidarDni("4587123a").ToString());
            Caso("CP-05", "DNI vacio", "False", Validaciones.ValidarDni("").ToString());
            Caso("CP-06", "Nombre valido (3 palabras)", "True", Validaciones.ValidarNombre("Quispe Rojas Maria").ToString());
            Caso("CP-07", "Nombre de una sola palabra", "False", Validaciones.ValidarNombre("Quispe").ToString());
            Caso("CP-08", "Nombre con numero", "False", Validaciones.ValidarNombre("Quispe R0jas").ToString());
            Caso("CP-09", "Nombre con tilde y enie", "True", Validaciones.ValidarNombre("Ñañez Pérez José").ToString());
            Caso("CP-10", "Celular valido", "True", Validaciones.ValidarTelefono("987654321").ToString());
            Caso("CP-11", "Celular que no empieza en 9", "False", Validaciones.ValidarTelefono("887654321").ToString());
            Caso("CP-12", "Celular con 8 digitos", "False", Validaciones.ValidarTelefono("98765432").ToString());
            Caso("CP-13", "Tipo de tramite 4", "True", Validaciones.ValidarTipo(4).ToString());
            Caso("CP-14", "Tipo de tramite 5", "False", Validaciones.ValidarTipo(5).ToString());
            Caso("CP-15", "Fecha valida 18/10/2026", "True", Validaciones.ValidarFecha("18/10/2026").ToString());
            Caso("CP-16", "Fecha inexistente 31/02/2026", "False", Validaciones.ValidarFecha("31/02/2026").ToString());

            Console.WriteLine("--- Registro ---");
            Almacen.Limpiar();
            var r1 = Expedientes.Registrar("45871236", "Quispe Rojas Maria", "987654321", 1, "18/10/2026");
            Caso("CP-17", "Registrar expediente valido (codigo)", "EXP-2026-0001", r1.Mensaje);
            var r2 = Expedientes.Registrar("12345678", "Abanto Diaz Rosa", "912345678", 2, "18/10/2026");
            Caso("CP-18", "Segundo expediente con codigo correlativo", "EXP-2026-0002", r2.Mensaje);
            var r3 = Expedientes.Registrar("23456789", "Mendoza Cruz Luis", "923456789", 3, "18/10/2026");
            var rDup = Expedientes.Registrar("45871236", "Quispe Rojas Maria", "987654321", 1, "18/10/2026");
            Caso("CP-19", "Rechazar duplicado (DNI+tramite+fecha)", "False", rDup.Exito.ToString());
            var rMal = Expedientes.Registrar("123", "Perez Juan", "987654321", 1, "18/10/2026");
            Caso("CP-20", "Rechazar DNI invalido al registrar", "False", rMal.Exito.ToString());
            Caso("CP-21", "Total de expedientes tras los registros", "3", Almacen.Total.ToString());

            Console.WriteLine("--- Busqueda ---");
            Caso("CP-22", "Buscar por codigo EXP-2026-0002 (posicion)", "1", Expedientes.ConsultarPorCodigo("EXP-2026-0002").ToString());
            Caso("CP-23", "Buscar codigo inexistente", "-1", Expedientes.ConsultarPorCodigo("EXP-2026-0099").ToString());
            Caso("CP-24", "Buscar por DNI 23456789 (posicion)", "2", Expedientes.ConsultarPorDni("23456789").ToString());

            Console.WriteLine("--- Ordenamiento ---");
            Expedientes.Ordenar(2);
            Caso("CP-25", "Ordenar por nombre: primero es Abanto", "Abanto Diaz Rosa", Almacen.Nombres[0]);
            Caso("CP-26", "Tras ordenar, el DNI sigue unido a su nombre", "12345678", Almacen.Dnis[0]);
            Expedientes.Ordenar(1);
            Caso("CP-27", "Ordenar por codigo: primero EXP-2026-0001", "EXP-2026-0001", Almacen.Codigos[0]);
            Caso("CP-28", "Busqueda binaria de EXP-2026-0003", "2", BusquedaOrden.BuscarBinaria(Almacen.Codigos, "EXP-2026-0003").ToString());

            Console.WriteLine("--- Persistencia ---");
            string ruta = Path.Combine(Path.GetTempPath(), "expedientes_prueba.txt");
            Archivos.Guardar(ruta);
            Almacen.Limpiar();
            int omitidas = Archivos.Cargar(ruta);
            Caso("CP-29", "Recuperar expedientes desde archivo", "3", Almacen.Total.ToString());
            Caso("CP-30", "Dato recuperado correcto (nombre del 1.o)", "Quispe Rojas Maria", Almacen.Nombres[0]);
            File.AppendAllText(ruta, "linea danada sin separadores" + Environment.NewLine);
            Archivos.Cargar(ruta);
            Caso("CP-31", "Omitir linea danada al cargar", "3", Almacen.Total.ToString());
            File.Delete(ruta);

            Console.WriteLine();
            Console.WriteLine("RESULTADO: " + aprobados + " de " + total + " casos aprobados.");
        }
    }
}
