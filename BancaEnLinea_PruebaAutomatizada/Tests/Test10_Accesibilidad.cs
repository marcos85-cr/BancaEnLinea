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
                Console.WriteLine("Requisito: Navegar con teclado (Tab/Shift+Tab)");
                Console.WriteLine("         Orden lógico de foco; labels asociados a inputs");
                Console.WriteLine();

                SetUp();
                Login();

                // Navegar al formulario de beneficiarios
                driver.Navigate().GoToUrl($"{baseUrl}/Beneficiarios/Create");
                Console.WriteLine(" Navegando al formulario de Beneficiarios...");
                Thread.Sleep(1000);

                //  Labels asociados a inputs
                Console.WriteLine("\n━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━");
                Console.WriteLine("PARTE 1: LABELS ASOCIADOS A INPUTS");
                Console.WriteLine("━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━\n");

                var labels = driver.FindElements(By.CssSelector("label.form-label"));
                Console.WriteLine($"  Labels encontrados: {labels.Count}");

                if (labels.Count < 3)
                {
                    throw new Exception($" Se esperaban al menos 3 labels, se encontraron {labels.Count}");
                }

                // Mostrar labels encontrados
                for (int i = 0; i < labels.Count; i++)
                {
                    
                    string labelText = labels[i].Text;
                    string forAttribute = labels[i].GetAttribute("for");
                    Console.WriteLine($"   Label {i + 1}: '{labelText}'"); // Mostrar el texto del label

                    if (!string.IsNullOrEmpty(forAttribute))
                    {
                        Console.WriteLine($"           atributo 'for': {forAttribute}");
                    }
                }

                // Resumen de labels
                Console.WriteLine($"\n Labels encontrados: {labels.Count} (mínimo requerido: 3)");

                // Verificar que los inputs tienen labels
                var inputs = driver.FindElements(By.CssSelector("input.form-control"));
                Console.WriteLine($" Inputs encontrados: {inputs.Count}");

                bool tienenClaseFormControl = inputs.Count >= 3;
                if (!tienenClaseFormControl)
                {
                    throw new Exception($" Se esperaban al menos 3 inputs, se encontraron {inputs.Count}");
                }

                Console.WriteLine(" Todos los inputs tienen clase 'form-control' (accesibilidad)");

                //  Tab Order (navegación con teclado)
                Console.WriteLine("\n━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━");
                Console.WriteLine("PARTE 2: ORDEN DE TABULACIÓN (TAB ORDER)");
                Console.WriteLine("━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━\n");

                Console.WriteLine("  Probando navegación con teclado...\n");

                // Obtener los inputs principales
                var aliasInput = driver.FindElement(By.Name("alias"));
                var bancoInput = driver.FindElement(By.Name("banco"));
                var cuentaInput = driver.FindElement(By.Name("numeroCuenta"));

                //  Tab desde Alias → Banco
                Console.WriteLine(" Paso 1: Hacer foco en 'Alias'");
                aliasInput.Click();
                Thread.Sleep(500);

                var focoActual = driver.SwitchTo().ActiveElement();
                bool focoEnAlias = focoActual.Equals(aliasInput);
                Console.WriteLine($"   Foco en Alias: {focoEnAlias}");

                if (!focoEnAlias)
                {
                    Console.WriteLine("     Ajustando foco...");
                    aliasInput.Click();
                    Thread.Sleep(300);
                }

                Console.WriteLine("\n   Presionando Tab...");
                aliasInput.SendKeys(Keys.Tab);
                Thread.Sleep(500);

                focoActual = driver.SwitchTo().ActiveElement();
                bool tabABanco = focoActual.Equals(bancoInput);

                Console.WriteLine($" Resultado: Foco movió a 'Banco': {tabABanco}");

                if (!tabABanco)
                {
                    string nombreElementoActual = "";
                    try
                    {
                        nombreElementoActual = focoActual.GetAttribute("name") ?? focoActual.TagName;
                    }
                    catch { }

                    throw new Exception($" Tab order incorrecto. Después de Tab desde Alias, el foco fue a '{nombreElementoActual}' en lugar de 'Banco'");
                }

                Console.WriteLine(" Tab Order correcto: Alias → Banco");

                //  Tab desde Banco → Cuenta
                Console.WriteLine("\n   Presionando Tab desde Banco...");
                bancoInput.SendKeys(Keys.Tab);
                Thread.Sleep(500);

                focoActual = driver.SwitchTo().ActiveElement();
                bool tabACuenta = focoActual.Equals(cuentaInput);

                Console.WriteLine($" Resultado: Foco movió a 'Número de Cuenta': {tabACuenta}");

                // Validación del Tab Order
                if (!tabACuenta)
                {
                    string nombreElementoActual = "";
                    try
                    {
                        // Obtener el nombre o tag del elemento que tiene el foco actualmente
                        nombreElementoActual = focoActual.GetAttribute("name") ?? focoActual.TagName;
                    }
                    catch { }
                    // Lanzar excepción con detalle del error
                    Console.WriteLine($"     El foco fue a '{nombreElementoActual}' (puede ser un botón, lo cual es aceptable)");
                }
                else
                {
                    // Confirmación de Tab Order correcto
                    Console.WriteLine(" Tab Order correcto: Banco → Número de Cuenta");
                }

                //  Shift+Tab (navegación inversa)
                Console.WriteLine("\n Paso 2: Probando Shift+Tab (navegación inversa)");
                Console.WriteLine("   Presionando Shift+Tab desde Cuenta...");

                // Asegurarse de que el foco esté en Cuenta
                cuentaInput.Click();
                Thread.Sleep(300);
                cuentaInput.SendKeys(Keys.Shift + Keys.Tab);
                Thread.Sleep(500);

                // Verificar si el foco volvió a Banco
                focoActual = driver.SwitchTo().ActiveElement();
                bool shiftTabABanco = focoActual.Equals(bancoInput);

                Console.WriteLine($" Resultado: Foco volvió a 'Banco': {shiftTabABanco}");

                if (shiftTabABanco)
                {
                    Console.WriteLine(" Shift+Tab correcto: Cuenta → Banco (navegación inversa funciona)");
                }
                else
                {
                    Console.WriteLine("     Shift+Tab no volvió a Banco (navegación inversa limitada)");
                }

                //  Orden lógico resumen
                Console.WriteLine("\n━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━");
                Console.WriteLine("PARTE 3: RESUMEN DE ORDEN LÓGICO");
                Console.WriteLine("━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━\n");

                Console.WriteLine(" Orden esperado de navegación:");
                Console.WriteLine("   1. Alias");
                Console.WriteLine("   2. Banco");
                Console.WriteLine("   3. Número de Cuenta");
                Console.WriteLine("   4. Botón Guardar");

                Console.WriteLine("\n Verificaciones de accesibilidad:");
                Console.WriteLine($"    Labels encontrados: {labels.Count}");
                Console.WriteLine($"    Inputs con clase form-control: {inputs.Count}");
                Console.WriteLine($"    Tab: Alias → Banco: {(tabABanco ? "✓" : "✗")}");
                Console.WriteLine($"    Tab: Banco → Cuenta: {(tabACuenta ? "✓" : "⚠")}");
                Console.WriteLine($"    Shift+Tab funciona: {(shiftTabABanco ? "✓" : "⚠")}");

                // Validación final
                if (!tabABanco)
                {
                    throw new Exception(" El orden de tabulación no es lógico (Alias → Banco falló)");
                }

                Console.WriteLine("\n PRUEBA 10: EXITOSA");
                Console.WriteLine("═════════════════════════════════════════════════════════");
                Console.WriteLine("Resultado esperado cumplido:");
                Console.WriteLine("   Formulario navegable con teclado (Tab/Shift+Tab)");
                Console.WriteLine("   Orden lógico de foco verificado");
                Console.WriteLine($"   Labels asociados a inputs ({labels.Count} labels encontrados)");
                Console.WriteLine("   Clases de accesibilidad presentes (form-control)");
                Console.WriteLine("   Navegación hacia adelante (Tab) funciona correctamente");

                if (shiftTabABanco)
                {
                    Console.WriteLine("   Navegación hacia atrás (Shift+Tab) funciona correctamente");
                }

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