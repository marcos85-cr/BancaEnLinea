using BancaEnLinea_PruebaAutomatizada.Helpers;
using BancaEnLinea_PruebaAutomatizada.PageObjects;

namespace BancaEnLinea_PruebaAutomatizada.Tests
{
    public class Test04_TransferenciaValida : BaseTest
    {
        // Método principal que ejecuta la prueba
        public void Ejecutar()
        {
            bool passed = false;
            try
            {
                Console.WriteLine("\n═════════════════════════════════════════════════════════════════");
                Console.WriteLine("  PRUEBA 4: Transferencia válida (actualización visual de saldos)");
                Console.WriteLine("════════════════════════════════════════════════════════════════════\n");

                SetUp();
                Login();

                // Inicializar Page Objects
                var dashboardPage = new DashboardPage(driver, wait);
                var transferenciasPage = new TransferenciasPage(driver, wait);
                decimal montoTransferencia = 1000m;

                // Obtener saldos iniciales
                driver.Navigate().GoToUrl($"{baseUrl}/Dashboard");
                var saldosIniciales = dashboardPage.GetAccountBalances();
                Console.WriteLine($" Saldos iniciales:");
                for (int i = 0; i < saldosIniciales.Count; i++)
                {
                    Console.WriteLine($"   Cuenta {i + 1}: {saldosIniciales[i]}");
                }

                // Realizar transferencia
                driver.Navigate().GoToUrl($"{baseUrl}/Transferencias/Create");
                Console.WriteLine($"\n Transfiriendo ₡{montoTransferencia}...");

                transferenciasPage.SelectCuentaOrigen(0);
                transferenciasPage.SelectCuentaDestino(1);
                transferenciasPage.EnterMonto(montoTransferencia);
                transferenciasPage.ClickTransferir();

                wait.Until(d => d.Url.Contains("Accounts/Overview"));

                // Verificar saldos finales
                driver.Navigate().GoToUrl($"{baseUrl}/Dashboard");
                var saldosFinales = dashboardPage.GetAccountBalances();
                Console.WriteLine($"\n Saldos finales:");
                for (int i = 0; i < saldosFinales.Count; i++)
                {
                    Console.WriteLine($"   Cuenta {i + 1}: {saldosFinales[i]}");
                }

                if (saldosIniciales.Count != 2 || saldosFinales.Count != 2)
                {
                    throw new Exception(" Debe haber exactamente 2 cuentas");
                }

                if (saldosFinales[0] == saldosIniciales[0])
                {
                    throw new Exception(" El saldo de la cuenta origen no cambió");
                }
                
                
                Console.WriteLine("\n PRUEBA 4: EXITOSA");
                passed = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\n PRUEBA 4: FALLIDA");
                Console.WriteLine($"Error: {ex.Message}");
            }
            finally
            {
                TearDown("Test04_TransferenciaValida", passed);
            }
        }
    }
}