using BancaEnLinea_PruebaAutomatizada.Helpers;
using BancaEnLinea_PruebaAutomatizada.PageObjects;

namespace BancaEnLinea_PruebaAutomatizada.Tests
{
    public class Test06_PagoServicio : BaseTest
    {
        public void Ejecutar()
        {
            bool passed = false;
            try
            {
                Console.WriteLine("\n════════════════════════════════════════════════════════");
                Console.WriteLine("  PRUEBA 6: Pago de servicio y apertura de comprobante");
                Console.WriteLine("══════════════════════════════════════════════════════════\n");

                SetUp();
                Login();

                // Preparar datos de pago
                var pagosPage = new PagosPage(driver, wait);
                string servicio = "Luz";
                string referencia = "REF-12345";
                decimal monto = 5000m;

                // Mostrar datos del pago
                Console.WriteLine($" Datos del pago:");
                Console.WriteLine($"   Servicio: {servicio}");
                Console.WriteLine($"   Referencia: {referencia}");
                Console.WriteLine($"   Monto: ₡{monto}");

                // Navegar a la página de pagos
                driver.Navigate().GoToUrl($"{baseUrl}/Pagos/Create");

                // Rellenar el formulario de pago
                pagosPage.SelectCuentaOrigen(0);
                pagosPage.EnterServicio(servicio);
                pagosPage.EnterReferencia(referencia);
                pagosPage.EnterMonto(monto);

                // Iniciar el pago
                Console.WriteLine("\n Procesando pago...");
                pagosPage.ClickPagar();

                // Verificar que el comprobante se muestra correctamente
                bool comprobanteVisible = pagosPage.IsComprobanteDisplayed();
                Console.WriteLine($" Comprobante generado: {comprobanteVisible}");

                // Validaciones finales
                if (!comprobanteVisible)
                {
                    throw new Exception(" El comprobante no se generó correctamente");
                }

                if (!driver.PageSource.Contains("ID de operación"))
                {
                    throw new Exception(" El comprobante no contiene ID de operación");
                }

                Console.WriteLine("\n PRUEBA 6: EXITOSA");
                passed = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\n PRUEBA 6: FALLIDA");
                Console.WriteLine($"Error: {ex.Message}");
            }
            finally
            {
                TearDown("Test06_PagoServicio", passed);
            }
        }
    }
}
