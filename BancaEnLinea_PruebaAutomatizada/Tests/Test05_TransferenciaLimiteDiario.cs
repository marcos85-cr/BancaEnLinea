using BancaEnLinea_PruebaAutomatizada.Helpers;
using BancaEnLinea_PruebaAutomatizada.PageObjects;

namespace BancaEnLinea_PruebaAutomatizada.Tests
{
    public class Test05_TransferenciaLimiteDiario : BaseTest
    {
        public void Ejecutar()
        {
            // Variable para rastrear el estado de la prueba
            bool passed = false;
            try
            {
                Console.WriteLine("\n═══════════════════════════════════════════════════════");
                Console.WriteLine("  PRUEBA 5: Transferencia rechazada por límite diario");
                Console.WriteLine("═════════════════════════════════════════════════════════\n");

                SetUp();
                Login();

                // Inicializar página de transferencias
                var transferenciasPage = new TransferenciasPage(driver, wait);
                decimal montoExcesivo = 250000m;

                // Mostrar detalles de la prueba
                Console.WriteLine($" Monto a transferir: ₡{montoExcesivo}");
                Console.WriteLine($" Límite diario: ₡200,000");

                // Navegar a la página de transferencias
                driver.Navigate().GoToUrl($"{baseUrl}/Transferencias/Create");

                // Realizar transferencia
                transferenciasPage.SelectCuentaOrigen(0);
                transferenciasPage.SelectCuentaDestino(1);
                transferenciasPage.EnterMonto(montoExcesivo);
                transferenciasPage.ClickTransferir();

                Thread.Sleep(1000);

                // Verificar mensaje de error
                var errorMsg = transferenciasPage.GetErrorMessage();
                Console.WriteLine($" Mensaje de error: {errorMsg}");

                if (!errorMsg.ToLower().Contains("límite diario excedido"))
                {
                    throw new Exception(" No se mostró mensaje de límite excedido");
                }

                // Si llegamos aquí, la prueba fue exitosa
                Console.WriteLine("\n PRUEBA 5: EXITOSA");
                passed = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\n PRUEBA 5: FALLIDA");
                Console.WriteLine($"Error: {ex.Message}");
            }
            finally
            {
                TearDown("Test05_TransferenciaLimiteDiario", passed);
            }
        }
    }
}