using BancaEnLinea_PruebaAutomatizada.Tests;

namespace BancaEnLinea_PruebaAutomatizada
{
    class Program
    {
        static void Main(string[] args)
        {
            // Encabezado del sistema de pruebas
            Console.Clear();
            Console.WriteLine("╔════════════════════════════════════════════════════════════╗");
            Console.WriteLine("║                                                            ║");
            Console.WriteLine("║    SISTEMA DE PRUEBAS AUTOMATIZADAS - BANCA EN LÍNEA       ║");
            Console.WriteLine("║                                                            ║");
            Console.WriteLine("╚════════════════════════════════════════════════════════════╝");
            Console.WriteLine();
            Console.WriteLine("Universidad Internacional de las Américas");
            Console.WriteLine("Curso: Verificación y Validación de Software");
            Console.WriteLine();
            Console.WriteLine("════════════════════════════════════════════════════════════");
            Console.WriteLine();

            // Verificar requisitos previos
            Console.WriteLine("  REQUISITOS PREVIOS:");
            Console.WriteLine("   1. Chrome instalado");
            Console.WriteLine("   2. Aplicación BancaEnLinea corriendo");
            Console.WriteLine("   3. URL: https://localhost:61156");
            Console.WriteLine();
            Console.Write("¿Está la aplicación corriendo? (S/N): ");

            // Verificar si la aplicación está corriendo
            var respuesta = Console.ReadLine()?.ToUpper();
            if (respuesta != "S")
            {
                // Instrucciones para iniciar la aplicación
                Console.WriteLine("\n  Por favor, inicie la aplicación primero:");
                Console.WriteLine("   cd BancaEnLinea");
                Console.WriteLine("   dotnet run");
                Console.WriteLine("\nPresione cualquier tecla para salir...");
                Console.ReadKey();
                return;
            }

            // Menú de selección de prueba
            Console.WriteLine("\n════════════════════════════════════════════════════════════");
            Console.WriteLine("  MENÚ DE PRUEBAS");
            Console.WriteLine("════════════════════════════════════════════════════════════");
            Console.WriteLine();
            Console.WriteLine("  1.  Login y verificación de nombre");
            Console.WriteLine("  2.  Alta de beneficiario");
            Console.WriteLine("  3.  Validación de cuenta inválida");
            Console.WriteLine("  4.  Transferencia válida");
            Console.WriteLine("  5.  Transferencia rechazada por límite diario");
            Console.WriteLine("  6.  Pago de servicio y comprobante");
            Console.WriteLine("  7.  Historial con filtros");
            Console.WriteLine("  8.  Exportación CSV");
            Console.WriteLine("  9.  Edición y eliminación de beneficiario");
            Console.WriteLine("  10. Accesibilidad básica");
            Console.WriteLine("  11. EJECUTAR TODAS LAS PRUEBAS");
            Console.WriteLine("  0.  Salir");
            Console.WriteLine();
            Console.Write("Seleccione una opción: ");

            var opcion = Console.ReadLine();

            Console.WriteLine("\n════════════════════════════════════════════════════════════");
            Console.WriteLine();

            var horaInicio = DateTime.Now;

            // Ejecución de la prueba seleccionada
            switch (opcion)
            {
                case "1":
                    new Test01_Login().Ejecutar();
                    break;
                case "2":
                    new Test02_AltaBeneficiario().Ejecutar();
                    break;
                case "3":
                    new Test03_ValidacionCuentaInvalida().Ejecutar();
                    break;
                case "4":
                    new Test04_TransferenciaValida().Ejecutar();
                    break;
                case "5":
                    new Test05_TransferenciaLimiteDiario().Ejecutar();
                    break;
                case "6":
                    new Test06_PagoServicio().Ejecutar();
                    break;
                case "7":
                    new Test07_HistorialFiltros().Ejecutar();
                    break;
                case "8":
                    new Test08_ExportacionCSV().Ejecutar();
                    break;
                case "9":
                    new Test09_EdicionEliminacion().Ejecutar();
                    break;
                case "10":
                    new Test10_Accesibilidad().Ejecutar();
                    break;
                case "11":
                    EjecutarTodasLasPruebas();
                    break;
                case "0":
                    Console.WriteLine("👋 ¡Hasta luego!");
                    return;
                default:
                    Console.WriteLine("❌ Opción inválida");
                    break;
            }

            // Cálculo de duración total
            var horaFin = DateTime.Now;
            var duracion = horaFin - horaInicio;

            // Resumen de duración total
            Console.WriteLine("\n════════════════════════════════════════════════════════════");
            Console.WriteLine($"  Duración total: {duracion.TotalSeconds:F2} segundos");
            Console.WriteLine("════════════════════════════════════════════════════════════");
            Console.WriteLine("\n Las capturas de pantalla se guardaron en la carpeta Screenshots");
            Console.WriteLine("\nPresione cualquier tecla para salir...");
            Console.ReadKey();
        }

        static void EjecutarTodasLasPruebas()
        {
            Console.WriteLine(" EJECUTANDO TODAS LAS PRUEBAS...\n");

            int exitosas = 0;
            int fallidas = 0;

            // Lista de pruebas a ejecutar
            var pruebas = new List<(string nombre, Action test)>
            {
                ("Test 1", () => new Test01_Login().Ejecutar()),
                ("Test 2", () => new Test02_AltaBeneficiario().Ejecutar()),
                ("Test 3", () => new Test03_ValidacionCuentaInvalida().Ejecutar()),
                ("Test 4", () => new Test04_TransferenciaValida().Ejecutar()),
                ("Test 5", () => new Test05_TransferenciaLimiteDiario().Ejecutar()),
                ("Test 6", () => new Test06_PagoServicio().Ejecutar()),
                ("Test 7", () => new Test07_HistorialFiltros().Ejecutar()),
                ("Test 8", () => new Test08_ExportacionCSV().Ejecutar()),
                ("Test 9", () => new Test09_EdicionEliminacion().Ejecutar()),
                ("Test 10", () => new Test10_Accesibilidad().Ejecutar())
            };

            foreach (var (nombre, test) in pruebas)
            {
                try
                {
                    test();
                    exitosas++;
                }
                catch
                {
                    fallidas++;
                }
                Console.WriteLine("\n" + new string('─', 60) + "\n");
            }

            Console.WriteLine("\n╔════════════════════════════════════════════════════════════╗");
            Console.WriteLine("║                   RESUMEN DE PRUEBAS                       ║");
            Console.WriteLine("╚════════════════════════════════════════════════════════════╝");
            Console.WriteLine();
            Console.WriteLine($"  Total de pruebas: {pruebas.Count}");
            Console.WriteLine($"  ✅ Exitosas: {exitosas}");
            Console.WriteLine($"  ❌ Fallidas: {fallidas}");
            Console.WriteLine($"  📊 Porcentaje de éxito: {(exitosas * 100.0 / pruebas.Count):F2}%");
            Console.WriteLine();
        }
    }
}