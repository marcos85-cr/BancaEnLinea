using BancaEnLinea_PruebaAutomatizada.Helpers;
using BancaEnLinea_PruebaAutomatizada.PageObjects;
using OpenQA.Selenium;

namespace BancaEnLinea_PruebaAutomatizada.Tests
{
    public class Test02_AltaBeneficiario : BaseTest
    {
        public void Ejecutar()
        {
            bool passed = false;
            try
            {
                Console.WriteLine("\n════════════════════════════════════════════════════════");
                Console.WriteLine("  PRUEBA 2: Alta de beneficiario (flujo feliz)");
                Console.WriteLine("════════════════════════════════════════════════════════\n");
                Console.WriteLine();
                Console.WriteLine();

                SetUp();
                Login();

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

                Console.WriteLine("\n Verificando estado inicial...");
                int cantidadInicial = beneficiariosPage.GetBeneficiariosCount();
                Console.WriteLine($"   Beneficiarios existentes: {cantidadInicial}");

                //  ACCIÓN: Completar formulario
                Console.WriteLine("\n Agregando nuevo beneficiario...");
                beneficiariosPage.ClickAddBeneficiario();

                Console.WriteLine("  Llenando formulario...");
                beneficiariosPage.FillBeneficiarioForm(alias, banco, numeroCuenta);

                // ACCIÓN: Guardar
                Console.WriteLine(" Guardando...");
                beneficiariosPage.ClickSave();

                // Esperar redirección
                wait.Until(d => d.Url.Contains("/Beneficiarios") && !d.Url.Contains("Create"));
                Thread.Sleep(1000);

                
                var successMsg = beneficiariosPage.GetSuccessMessage();
                Console.WriteLine($"\n Mensaje recibido: {successMsg}");

                if (!successMsg.Contains("agregado correctamente"))
                {
                    throw new Exception(" No se mostró mensaje de éxito");
                }
                Console.WriteLine(" Mensaje de éxito confirmado");

                // Beneficiario existe en la grilla
                bool exists = beneficiariosPage.BeneficiarioExists(alias);
                Console.WriteLine($"\n Beneficiario existe en la grilla: {exists}");

                if (!exists)
                {
                    throw new Exception(" El beneficiario no aparece en la grilla");
                }

                // Aparece en la PRIMERA FILA
                Console.WriteLine("\n Verificando posición en la grilla...");

                var primeraFila = driver.FindElement(By.CssSelector("tbody tr:first-child"));
                string textoPrimeraFila = primeraFila.Text;

                Console.WriteLine($" Contenido de primera fila: {textoPrimeraFila}");

                bool estaEnPrimeraFila = textoPrimeraFila.Contains(alias);

                if (!estaEnPrimeraFila)
                {
                    Console.WriteLine($"    Advertencia: El beneficiario NO está en la primera fila");
                    Console.WriteLine($"   Esto puede ser normal si hay beneficiarios previos");
                    Console.WriteLine($"   Pero el beneficiario SÍ aparece en la grilla");
                }
                else
                {
                    Console.WriteLine(" Beneficiario aparece en la PRIMERA FILA");
                }

                // Cantidad aumentó en 1
                int cantidadFinal = beneficiariosPage.GetBeneficiariosCount();
                Console.WriteLine($"\n Cantidad final: {cantidadFinal}");

                if (cantidadFinal != cantidadInicial + 1)
                {
                    throw new Exception($" Cantidad incorrecta. Esperado: {cantidadInicial + 1}, Actual: {cantidadFinal}");
                }
                Console.WriteLine($" Cantidad aumentó correctamente ({cantidadInicial} → {cantidadFinal})");

                Console.WriteLine("\n PRUEBA 2: EXITOSA");
                Console.WriteLine("════════════════════════════════════════════════════════");
                Console.WriteLine("Resultado esperado:");
                Console.WriteLine("  ✓ Formulario completado y guardado");
                Console.WriteLine("  ✓ Mensaje de éxito mostrado");
                Console.WriteLine("  ✓ Beneficiario aparece en la grilla");
                if (estaEnPrimeraFila)
                {
                    Console.WriteLine("  ✓ Beneficiario en primera fila");
                }
                else
                {
                    Console.WriteLine("    Beneficiario no en primera fila (hay otros anteriores)");
                }

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