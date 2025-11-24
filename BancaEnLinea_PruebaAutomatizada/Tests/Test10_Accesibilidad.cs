using BancaEnLinea_PruebaAutomatizada.Helpers;
using OpenQA.Selenium;

namespace BancaEnLinea_PruebaAutomatizada.Tests
{
    public class Test10_Accesibilidad : BaseTest
    {
        public void Ejecutar()
        {
            bool passed = false;
            try
            {
                Console.WriteLine("\n════════════════════════════════════════════════════════");
                Console.WriteLine("  PRUEBA 10: Accesibilidad Básica (tab order y labels)");
                Console.WriteLine("═════════════════════════════════════════════════════════\n");

                SetUp();
                Login();

                // Navegar a la página de creación de beneficiarios
                driver.Navigate().GoToUrl($"{baseUrl}/Beneficiarios/Create");
                Console.WriteLine(" Verificando formulario de beneficiarios...");

                // Localizar elementos del formulario   
                var aliasInput = driver.FindElement(By.Name("alias"));
                var bancoInput = driver.FindElement(By.Name("banco"));
                var cuentaInput = driver.FindElement(By.Name("numeroCuenta"));

                // Verificar labels asociados a los inputs
                var labels = driver.FindElements(By.CssSelector("label.form-label"));
                Console.WriteLine($"  Labels encontrados: {labels.Count}");

                // Verificar orden de tabulación (alias -> banco -> cuenta)
                Console.WriteLine("\n  Verificando orden de tabulación...");
                aliasInput.Click();
                aliasInput.SendKeys(Keys.Tab);
                Thread.Sleep(500);

                // Después de tabular desde alias, el foco debería estar en banco
                var activeElement = driver.SwitchTo().ActiveElement();
                bool tabOrderCorrecto = activeElement.Equals(bancoInput);
                Console.WriteLine($" Tab order correcto (alias → banco): {tabOrderCorrecto}");

                // Verificar clases de accesibilidad en los inputs
                bool tienenClaseFormControl = driver.FindElements(By.CssSelector("input.form-control")).Count >= 3;
                Console.WriteLine($" Inputs con clase form-control: {tienenClaseFormControl}");

                // Validaciones finales 
                if (labels.Count < 3)
                {
                    throw new Exception(" No se encontraron suficientes labels");
                }

                // Verificar orden de tabulación
                if (!tabOrderCorrecto)
                {
                    throw new Exception(" El orden de tabulación no es correcto");
                }

                // Verificar clases de accesibilidad
                if (!tienenClaseFormControl)
                {
                    throw new Exception(" Los inputs no tienen las clases apropiadas");
                }

                // Resultado de la prueba
                Console.WriteLine("\n PRUEBA 10: EXITOSA");
                passed = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\n PRUEBA 10: FALLIDA");
                Console.WriteLine($"Error: {ex.Message}");
            }
            finally
            {
                TearDown("Test10_Accesibilidad", passed);
            }
        }
    }
}
