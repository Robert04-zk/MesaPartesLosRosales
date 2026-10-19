# Mesa de partes digital – Consultorio LOS ROSALES (Cajamarca)

Prototipo en C# (.NET 8, consola) para el desafío "Del archivador al algoritmo" – Fundamentos de Programación (CIIN1205P).

## Módulos
| Archivo | Responsabilidad |
|---|---|
| Program.cs | Menú de consola y control del flujo |
| Validaciones.cs | Validar DNI, nombre, celular, tipo y fecha |
| Expedientes.cs | Arreglos paralelos (Almacen), registro, consulta y listado |
| BusquedaOrden.cs | Búsqueda lineal/binaria y ordenamiento por selección |
| Archivos.cs | Persistencia en `expedientes.txt` (campos separados por `|`) |
| Pruebas.cs | 31 casos de prueba automatizados |

## Cómo ejecutar
```
dotnet run                 # abre el menú
dotnet run -- pruebas      # ejecuta los casos de prueba
```
