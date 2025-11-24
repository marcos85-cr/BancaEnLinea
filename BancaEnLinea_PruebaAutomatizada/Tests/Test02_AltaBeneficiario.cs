using BancaEnLinea_PruebaAutomatizada.Helpers;
using BancaEnLinea_PruebaAutomatizada.PageObjects;

namespace BancaEnLinea_PruebaAutomatizada.Tests
{
    public class Test02_AltaBeneficiario : BaseTest
    {
        public void Ejecutar()
        {
            // Variable para rastrear el estado de la prueba
            bool passed = false;
            try
            {
                Console.WriteLine("\n════════════════════════════════════════");
                Console.WriteLine("  PRUEBA 2: Alta de Beneficiario");
                Console.WriteLine("════════════════════════════════════════\n");

                SetUp();
                Login();

                // Datos de prueba
                var beneficiariosPage = new BeneficiariosPage(driver, wait);
                string alias = $"Test Benef {DateTime.Now:HHmmss}";
                string banco = "Banco Nacional";
                string numeroCuenta = "12345678901234";

                Console.WriteLine($" Datos del beneficiario:");
                Console.WriteLine($"   Alias: {alias}");
                Console.WriteLine($"   Banco: {banco}");
                Console.WriteLine($"   Cuenta: {numeroCuenta}");

                // Navegar a la página de Beneficiarios
                driver.Navigate().GoToUrl($"{baseUrl}/Beneficiarios");
                beneficiariosPage.ClickAddBeneficiario();

                // Llenar el formulario
                Console.WriteLine("  Llenando formulario...");
                beneficiariosPage.FillBeneficiarioForm(alias, banco, numeroCuenta);
                beneficiariosPage.ClickSave();

                // Esperar a que se redirija a la lista de beneficiarios
                wait.Until(d => d.Url.Contains("/Beneficiarios") && !d.Url.Contains("Create"));

                // Verificar que el beneficiario fue agregado
                var successMsg = beneficiariosPage.GetSuccessMessage();
                bool exists = beneficiariosPage.BeneficiarioExists(alias);

                // Resultados de la prueba
                Console.WriteLine($" Mensaje: {successMsg}");
                Console.WriteLine($" Beneficiario existe en lista: {exists}");

                // Validaciones de la prueba  
                if (!successMsg.Contains("agregado correctamente"))
                {
                    throw new Exception(" No se mostró mensaje de éxito");
                }

                // Verificar que el beneficiario aparece en la grilla
                if (!exists)
                {
                    throw new Exception(" El beneficiario no aparece en la grilla");
                }

                Console.WriteLine("\n PRUEBA 2: EXITOSA");
                passed = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\n PRUEBA 2: FALLIDA");
                Console.WriteLine($"Error: {ex.Message}");
            }
            finally
            {
                TearDown("Test02_AltaBeneficiario", passed);
            }
        }
    }
}