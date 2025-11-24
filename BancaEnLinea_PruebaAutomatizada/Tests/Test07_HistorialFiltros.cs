using BancaEnLinea_PruebaAutomatizada.Helpers;
using BancaEnLinea_PruebaAutomatizada.PageObjects;

namespace BancaEnLinea_PruebaAutomatizada.Tests
{
    public class Test07_HistorialFiltros : BaseTest
    {
        // Método principal que ejecuta la prueba
        public void Ejecutar()
        {
            bool passed = false;
            try
            {
                Console.WriteLine("\n═══════════════════════════════════════════════════════");
                Console.WriteLine("  PRUEBA 7: Historial: filtros y verificación de filas");
                Console.WriteLine("══════════════════════════════════════════════════════════\n");

                SetUp();
                Login();

                var historialPage = new HistorialPage(driver, wait);
                var transferenciasPage = new TransferenciasPage(driver, wait);

                // Hacer una transferencia para tener datos en el historial
                Console.WriteLine(" Creando transferencia de prueba...");
                driver.Navigate().GoToUrl($"{baseUrl}/Transferencias/Create");
                transferenciasPage.SelectCuentaOrigen(0);
                transferenciasPage.SelectCuentaDestino(1);
                transferenciasPage.EnterMonto(500);
                transferenciasPage.ClickTransferir();

                // Probar filtros en el historial
                Console.WriteLine("\n Aplicando filtros...");
                driver.Navigate().GoToUrl($"{baseUrl}/Historial");

                // Seleccionar tipo "Transferencia"
                historialPage.SelectTipo("Transferencia");
                historialPage.ClickFiltrar();

                Thread.Sleep(1000);

                // Obtener resultados del historial
                int rowCount = historialPage.GetRowCount();
                var rowTypes = historialPage.GetRowTypes();

                // Mostrar resultados en consola
                Console.WriteLine($" Resultados:");
                Console.WriteLine($"   Filas encontradas: {rowCount}");
                Console.WriteLine($"   Tipos: {string.Join(", ", rowTypes)}");

                // Validaciones
                if (rowCount == 0)
                {
                    throw new Exception(" No se encontraron registros en el historial");
                }

                // Verificar que todas las filas sean del tipo Transferencia
                bool todosSonTransferencias = rowTypes.All(t => t.Contains("Transferencia"));
                if (!todosSonTransferencias)
                {
                    throw new Exception(" No todas las filas son del tipo Transferencia");
                }

                // Exportar resultados
                Console.WriteLine("\n PRUEBA 7: EXITOSA");
                passed = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\n PRUEBA 7: FALLIDA");
                Console.WriteLine($"Error: {ex.Message}");
            }
            finally
            {
                TearDown("Test07_HistorialFiltros", passed);
            }
        }
    }
}
