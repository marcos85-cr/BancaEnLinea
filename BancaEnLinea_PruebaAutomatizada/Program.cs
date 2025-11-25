using BancaEnLinea_PruebaAutomatizada.Tests;

namespace BancaEnLinea_PruebaAutomatizada
{
    class Program
    {
        static void Main(string[] args)
        {
            // Verificación inicial solo una vez
            Console.Clear();
            MostrarEncabezado();

            if (!VerificarAplicacionCorriendo())
            {
                return;
            }

            // Ciclo principal del menú
            bool continuar = true;
            while (continuar)
            {
                MostrarMenu();

                Console.Write("Seleccione una opción: ");
                var opcion = Console.ReadLine();

                Console.WriteLine("\n════════════════════════════════════════════════════════════");
                Console.WriteLine();

                var horaInicio = DateTime.Now;

                // Ejecutar la prueba seleccionada
                EjecutarOpcion(opcion);

                var horaFin = DateTime.Now;
                var duracion = horaFin - horaInicio;

                // Resumen de tiempo
                Console.WriteLine("\n════════════════════════════════════════════════════════════");
                Console.WriteLine($"  Duración: {duracion.TotalSeconds:F2} segundos");
                Console.WriteLine("════════════════════════════════════════════════════════════");
                Console.WriteLine("\n Las capturas se guardaron en: Screenshots\\");

                // Preguntar si desea continuar
                Console.WriteLine("\n¿Desea ejecutar otra prueba? (S/N): ");
                var respuesta = Console.ReadLine()?.ToUpper();

                if (respuesta != "S")
                {
                    continuar = false;
                    Console.WriteLine("\n ¡Hasta luego!");
                }
                else
                {
                    Console.Clear();
                    Console.WriteLine("════════════════════════════════════════════════════════════\n");
                }
            }
        }

        // Encabezado del proyecto
        static void MostrarEncabezado()
        {
            Console.WriteLine("╔════════════════════════════════════════════════════════════╗");
            Console.WriteLine("║                                                            ║");
            Console.WriteLine("║    SISTEMA DE PRUEBAS AUTOMATIZADAS - BANCA EN LÍNEA       ║");
            Console.WriteLine("║                                                            ║");
            Console.WriteLine("╚════════════════════════════════════════════════════════════╝");
            Console.WriteLine();
            Console.WriteLine("Universidad Internacional de las Américas");
            Console.WriteLine("Curso: Verificación y Validación de Software");
            Console.WriteLine("Grupo 6: Valeria, Hans y Marcos"); 
            Console.WriteLine("════════════════════════════════════════════════════════════");
            Console.WriteLine();
        }

        // Verificar si la aplicación está corriendo
        static bool VerificarAplicacionCorriendo()
        {
            // Instrucciones para el usuario
            Console.WriteLine("  REQUISITOS PREVIOS:");
            Console.WriteLine("   1. Chrome instalado");
            Console.WriteLine("   2. Aplicación BancaEnLinea corriendo");
            Console.WriteLine("   3. URL: https://localhost:61156");
            Console.WriteLine();
            Console.Write("¿Está la aplicación corriendo? (S/N): ");

            var respuesta = Console.ReadLine()?.ToUpper();
            if (respuesta != "S")
            {
                // Instrucciones para iniciar la aplicación
                Console.WriteLine("\n  Por favor, inicie la aplicación primero:");
                Console.WriteLine("   cd BancaEnLinea");
                Console.WriteLine("   dotnet run");
                Console.WriteLine("\nPresione cualquier tecla para salir...");
                Console.ReadKey();
                return false;
            }

            return true;
        }

        // Mostrar el menú de opciones disponibles
        static void MostrarMenu()
        {
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
        }

        // Ejecutar la prueba seleccionada
        static void EjecutarOpcion(string? opcion)
        {
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
                    Console.WriteLine(" ¡Hasta luego!");
                    Environment.Exit(0);
                    break;
                default:
                    Console.WriteLine(" Opción inválida");
                    break;
            }
        }

        // Ejecutar todas las pruebas secuencialmente y mostrar un resumen
        static void EjecutarTodasLasPruebas()
        {
            Console.WriteLine(" EJECUTANDO TODAS LAS PRUEBAS...\n");

            int exitosas = 0;
            int fallidas = 0;

            var pruebas = new List<(string nombre, Action test)>
            {
                // Agregar todas las pruebas aquí
                ("Test 1: Login", () => new Test01_Login().Ejecutar()),
                ("Test 2: Alta Beneficiario", () => new Test02_AltaBeneficiario().Ejecutar()),
                ("Test 3: Cuenta Inválida", () => new Test03_ValidacionCuentaInvalida().Ejecutar()),
                ("Test 4: Transferencia Válida", () => new Test04_TransferenciaValida().Ejecutar()),
                ("Test 5: Límite Diario", () => new Test05_TransferenciaLimiteDiario().Ejecutar()),
                ("Test 6: Pago Servicio", () => new Test06_PagoServicio().Ejecutar()),
                ("Test 7: Historial Filtros", () => new Test07_HistorialFiltros().Ejecutar()),
                ("Test 8: Exportación CSV", () => new Test08_ExportacionCSV().Ejecutar()),
                ("Test 9: Edición/Eliminación", () => new Test09_EdicionEliminacion().Ejecutar()),
                ("Test 10: Accesibilidad", () => new Test10_Accesibilidad().Ejecutar())
            };

            // Ejecutar cada prueba y capturar resultados 
            foreach (var (nombre, test) in pruebas)
            {
                Console.WriteLine($"\n  Ejecutando: {nombre}");
                Console.WriteLine(new string('─', 60));

                try
                {
                    test();
                    exitosas++;
                    Console.WriteLine($" {nombre}: EXITOSA");
                }
                catch (Exception ex)
                {
                    fallidas++;
                    Console.WriteLine($" {nombre}: FALLIDA - {ex.Message}");
                }

                Console.WriteLine(new string('─', 60));
            }

            // Mostrar resumen final  
            Console.WriteLine("\n╔══════════════════════════════════════════════════════════════╗");
            Console.WriteLine("  ║                   RESUMEN DE PRUEBAS                         ║");
            Console.WriteLine("  ╚══════════════════════════════════════════════════════════════╝");
            Console.WriteLine();
            Console.WriteLine($"   Total de pruebas: {pruebas.Count}");
            Console.WriteLine($"   Exitosas: {exitosas}");
            Console.WriteLine($"   Fallidas: {fallidas}");
            Console.WriteLine($"   Porcentaje de éxito: {(exitosas * 100.0 / pruebas.Count):F2}%");
            Console.WriteLine();
        }
    }
}
