using BancaEnLinea_PruebaAutomatizada.Helpers;
using BancaEnLinea_PruebaAutomatizada.PageObjects;

namespace BancaEnLinea_PruebaAutomatizada.Tests
{
    public class Test08_ExportacionCSV : BaseTest
    {
        public void Ejecutar()
        {
            bool passed = false;
            try
            {
                Console.WriteLine("\n════════════════════════════════════════");
                Console.WriteLine("  PRUEBA 8: Exportación simulada");
                Console.WriteLine("════════════════════════════════════════\n");

                // Configuración inicial y login
                SetUp();
                Login();

                // Inicializar la página de Historial 
                var historialPage = new HistorialPage(driver, wait);

                // Navegar a la página de Historial  
                driver.Navigate().GoToUrl($"{baseUrl}/Historial");
                Console.WriteLine(" Navegando a Historial...");

                // Aplicar filtros antes de exportar 
                Console.WriteLine(" Iniciando exportación...");
                historialPage.ClickExportar();

                Thread.Sleep(2000);

                // Verificar que la URL contiene "Export"
                if (!driver.Url.Contains("Export"))
                {
                    throw new Exception(" La URL no contiene 'Export'");
                }

                
                Console.WriteLine(" Descarga iniciada correctamente"); // Mensaje de éxito
                Console.WriteLine("\n PRUEBA 8: EXITOSA");
                passed = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\n PRUEBA 8: FALLIDA");
                Console.WriteLine($"Error: {ex.Message}");
            }
            finally
            {
                TearDown("Test08_ExportacionCSV", passed);
            }
        }
    }
}