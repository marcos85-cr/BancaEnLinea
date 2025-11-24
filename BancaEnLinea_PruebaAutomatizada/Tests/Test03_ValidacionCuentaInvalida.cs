using BancaEnLinea_PruebaAutomatizada.Helpers;
using BancaEnLinea_PruebaAutomatizada.PageObjects;

namespace BancaEnLinea_PruebaAutomatizada.Tests
{
    public class Test03_ValidacionCuentaInvalida : BaseTest
    {
        // Método principal que ejecuta la prueba
        public void Ejecutar()
        {
            bool passed = false;
            try
            {
                Console.WriteLine("\n═══════════════════════════════════════════════════════");
                Console.WriteLine("  PRUEBA 3: Validación de beneficiario con cuenta inválida");
                Console.WriteLine("════════════════════════════════════════════════════════════\n");

                SetUp();
                Login();

                // Navegar a la página de Beneficiarios
                var beneficiariosPage = new BeneficiariosPage(driver, wait);
                string alias = "Test Invalido";
                string banco = "Banco Popular";
                string numeroCuentaInvalido = "123ABC";

                // Intentar crear beneficiario con cuenta inválida
                Console.WriteLine($" Intentando crear beneficiario con cuenta inválida:");
                Console.WriteLine($"   Cuenta: {numeroCuentaInvalido}");

                driver.Navigate().GoToUrl($"{baseUrl}/Beneficiarios/Create");

                // Rellenar el formulario con datos inválidos
                beneficiariosPage.FillBeneficiarioForm(alias, banco, numeroCuentaInvalido);
                beneficiariosPage.ClickSave();

                // Esperar respuesta
                Thread.Sleep(1000);

                // Verificar que se muestra mensaje de error y no se crea el beneficiario
                var errorMsg = beneficiariosPage.GetErrorMessage();
                Console.WriteLine($" Mensaje de error: {errorMsg}");

                if (!errorMsg.Contains("inválida"))
                {
                    throw new Exception(" No se mostró mensaje de error de validación");
                }

                if (!driver.Url.Contains("Create"))
                {
                    throw new Exception(" Se redirigió cuando debería quedarse en el formulario");
                }

                Console.WriteLine("\n PRUEBA 3: EXITOSA");
                passed = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\n PRUEBA 3: FALLIDA");
                Console.WriteLine($"Error: {ex.Message}");
            }
            finally
            {
                TearDown("Test03_ValidacionCuentaInvalida", passed);
            }
        }
    }
}