using BancaEnLinea_PruebaAutomatizada.Helpers;
using BancaEnLinea_PruebaAutomatizada.PageObjects;
using OpenQA.Selenium;

namespace BancaEnLinea_PruebaAutomatizada.Tests
{
    public class Test03_ValidacionCuentaInvalida : BaseTest
    {
        public void Ejecutar()
        {
            bool passed = false;
            try
            {
                Console.WriteLine("\n═══════════════════════════════════════════════════════");
                Console.WriteLine("  PRUEBA 3: Validación de cuenta inválida");
                Console.WriteLine("════════════════════════════════════════════════════════\n");
                Console.WriteLine("Requisito: Mensaje de error junto al campo");
                Console.WriteLine();

                SetUp();
                Login();

                // Navegar a la página de Beneficiarios
                var beneficiariosPage = new BeneficiariosPage(driver, wait);
                string alias = "Test Invalido";
                string banco = "Banco Popular";
                string numeroCuentaInvalido = "123ABC";  

                Console.WriteLine($" Datos del beneficiario:");
                Console.WriteLine($"   Alias: {alias}");
                Console.WriteLine($"   Banco: {banco}");
                Console.WriteLine($"   Cuenta: {numeroCuentaInvalido}  (inválida)");

                // Enviar formato incorrecto
                driver.Navigate().GoToUrl($"{baseUrl}/Beneficiarios/Create");
                Console.WriteLine("\n  Llenando formulario con cuenta inválida...");

                beneficiariosPage.FillBeneficiarioForm(alias, banco, numeroCuentaInvalido);

                Console.WriteLine(" Intentando guardar...");
                beneficiariosPage.ClickSave();

                Thread.Sleep(1500); // Esperar a que se procese el envío

                //  Mensaje de error general
                var errorMsgGeneral = beneficiariosPage.GetErrorMessage();
                Console.WriteLine($"\n Buscando mensaje de error...");
                Console.WriteLine($"   Mensaje general (alert-danger): '{errorMsgGeneral}'");

                bool hayErrorGeneral = !string.IsNullOrEmpty(errorMsgGeneral) &&
                                      errorMsgGeneral.ToLower().Contains("inválida");

                // Mensaje de error junto al campo (validación HTML5 o personalizada)
                bool hayErrorJuntoCampo = false;
                string mensajeJuntoCampo = "";

                try
                {
                    // Intentar buscar mensaje de validación junto al input
                    var validationMessages = driver.FindElements(By.CssSelector(".invalid-feedback, .text-danger, .field-validation-error"));

                    if (validationMessages.Any())
                    {
                        mensajeJuntoCampo = string.Join(", ", validationMessages.Select(m => m.Text));
                        hayErrorJuntoCampo = true;
                        Console.WriteLine($"   Mensaje junto al campo: '{mensajeJuntoCampo}'");
                    }
                }
                catch { }

                // Verificar validación HTML5
                try
                {
                    var cuentaInput = driver.FindElement(By.Name("numeroCuenta"));
                    string validityState = (string)((IJavaScriptExecutor)driver).ExecuteScript(
                        "return arguments[0].validationMessage;", cuentaInput
                    );

                    if (!string.IsNullOrEmpty(validityState))
                    {
                        Console.WriteLine($"   Validación HTML5: '{validityState}'");
                        hayErrorJuntoCampo = true;
                        mensajeJuntoCampo = validityState;
                    }
                }
                catch { }

                    
                bool seQuedoEnFormulario = driver.Url.Contains("Create");
                Console.WriteLine($"\n Se quedó en el formulario: {seQuedoEnFormulario}");

                if (!seQuedoEnFormulario)
                {
                    throw new Exception(" Se redirigió cuando debería quedarse en el formulario");
                }

                //  El beneficiario NO se creó
                driver.Navigate().GoToUrl($"{baseUrl}/Beneficiarios");
                Thread.Sleep(1000);

                bool beneficiarioNoExiste = !beneficiariosPage.BeneficiarioExists(alias);
                Console.WriteLine($" Beneficiario NO se creó: {beneficiarioNoExiste}");

                if (!beneficiarioNoExiste)
                {
                    throw new Exception(" El beneficiario se creó cuando NO debería");
                }

                // Validación final
                if (!hayErrorGeneral && !hayErrorJuntoCampo)
                {
                    throw new Exception(" No se mostró ningún mensaje de error (ni general ni junto al campo)");
                }

                Console.WriteLine("\n PRUEBA 3: EXITOSA");
                Console.WriteLine("════════════════════════════════════════════════════════");
                Console.WriteLine("Resultado esperado:");
                Console.WriteLine("   Formulario con cuenta inválida enviado");

                if (hayErrorGeneral)
                {
                    Console.WriteLine($"   Mensaje de error general mostrado: '{errorMsgGeneral}'");
                }

                if (hayErrorJuntoCampo)
                {
                    Console.WriteLine($"   Mensaje junto al campo mostrado: '{mensajeJuntoCampo}'");
                }

                Console.WriteLine("   Se quedó en el formulario");
                Console.WriteLine("   Beneficiario NO se creó");

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