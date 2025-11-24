using BancaEnLinea_PruebaAutomatizada.Helpers;
using BancaEnLinea_PruebaAutomatizada.PageObjects;
using OpenQA.Selenium;

namespace BancaEnLinea_PruebaAutomatizada.Tests
{
    public class Test09_EdicionEliminacion : BaseTest
    {
        public void Ejecutar()
        {
            bool passed = false;
            try
            {
                Console.WriteLine("\n════════════════════════════════════════════════════");
                Console.WriteLine("  PRUEBA 9: Edición y Eliminación de Beneficiario");
                Console.WriteLine("══════════════════════════════════════════════════════\n");

                SetUp();
                Login();

                // Navegar a la página de Beneficiarios
                var beneficiariosPage = new BeneficiariosPage(driver, wait);
                string aliasOriginal = $"Benef Original {DateTime.Now:HHmmss}";
                string aliasEditado = $"Benef Editado {DateTime.Now:HHmmss}";

                // Crear beneficiario inicial
                Console.WriteLine($" Creando beneficiario: {aliasOriginal}");
                driver.Navigate().GoToUrl($"{baseUrl}/Beneficiarios/Create");
                beneficiariosPage.FillBeneficiarioForm(aliasOriginal, "Banco BCR", "12345678901234");
                beneficiariosPage.ClickSave();

                // Esperar a que se redirija a la lista de beneficiarios
                wait.Until(d => d.Url.Contains("/Beneficiarios") && !d.Url.Contains("Create"));

                // Contar beneficiarios iniciales antes de la edición y eliminación
                int countInicial = beneficiariosPage.GetBeneficiariosCount();
                Console.WriteLine($" Cantidad inicial: {countInicial}");

                // Editar beneficiario creado antes
                Console.WriteLine($"\n  Editando beneficiario a: {aliasEditado}");
                beneficiariosPage.ClickEditFirstBeneficiario();

                // Esperar a que se cargue la página de edición
                wait.Until(d => d.Url.Contains("Edit"));

                // Rellenar el formulario con los nuevos datos
                beneficiariosPage.FillBeneficiarioForm(aliasEditado, "Banco Popular", "98765432109876");
                beneficiariosPage.ClickSave();

                // Esperar a que se redirija a la lista de beneficiarios
                wait.Until(d => d.Url.Contains("/Beneficiarios") && !d.Url.Contains("Edit"));

                // Verificar que el alias editado existe en la lista
                bool existeEditado = beneficiariosPage.BeneficiarioExists(aliasEditado);
                Console.WriteLine($" Beneficiario editado existe: {existeEditado}");

                // Eliminar el beneficiario editado
                Console.WriteLine("\n  Eliminando beneficiario...");
                Thread.Sleep(1000);
                beneficiariosPage.ClickDeleteFirstBeneficiario();

                // Esperar a que aparezca el mensaje de éxito
                wait.Until(d => d.FindElements(By.CssSelector(".alert-success")).Count > 0);

                // Contar beneficiarios finales después de la eliminación
                int countFinal = beneficiariosPage.GetBeneficiariosCount();
                Console.WriteLine($" Cantidad final: {countFinal}");

                // Validaciones finales 
                if (!existeEditado)
                {
                    throw new Exception(" El alias no se actualizó correctamente");
                }

                if (countFinal >= countInicial)
                {
                    throw new Exception(" El beneficiario no fue eliminado");
                }

                // Si todas las validaciones pasan
                Console.WriteLine("\n PRUEBA 9: EXITOSA");
                passed = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\n PRUEBA 9: FALLIDA");
                Console.WriteLine($"Error: {ex.Message}");
            }
            finally
            {
                TearDown("Test09_EdicionEliminacion", passed);
            }
        }
    }
}
