using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

namespace BancaEnLinea_PruebaAutomatizada.Helpers
{
    public class BaseTest
    {
        protected IWebDriver driver = null!;
        protected WebDriverWait wait = null!;
        protected string baseUrl = "https://localhost:61156"; // ⚠️ Ajusta según tu puerto

        // Configurar el navegador antes de cada prueba
        public virtual void SetUp()
        {
            Console.WriteLine(" Configurando Chrome WebDriver...");

            var options = new ChromeOptions();

            // Argumentos para Chrome (optimización y seguridad)
            options.AddArgument("--start-maximized");
            options.AddArgument("--disable-extensions");
            options.AddArgument("--ignore-certificate-errors");
            options.AddArgument("--allow-insecure-localhost");
            options.AddArgument("--ignore-ssl-errors=yes");
            options.AddArgument("--no-sandbox");
            options.AddArgument("--disable-dev-shm-usage");
            options.AddArgument("--disable-gpu");
            options.AddArgument("--remote-debugging-port=9222");
            options.AddArgument("--disable-blink-features=AutomationControlled");

            // Aceptar certificados no seguros (crítico para HTTPS en desarrollo)
            options.AcceptInsecureCertificates = true;

            // Preferencias adicionales
            options.AddUserProfilePreference("download.default_directory",
                Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Downloads"));
            options.AddUserProfilePreference("download.prompt_for_download", false);
            options.AddUserProfilePreference("disable-popup-blocking", "true");

            try
            {
                var executingPath = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
                Console.WriteLine($" Ruta de ejecución: {executingPath}");

                // Configurar servicio de ChromeDriver
                var service = ChromeDriverService.CreateDefaultService(executingPath);
                service.HideCommandPromptWindow = true;
                service.SuppressInitialDiagnosticInformation = true;
                // Iniciar ChromeDriver
                driver = new ChromeDriver(service, options);
                Console.WriteLine(" Chrome configurado correctamente");
            }
            catch (Exception ex)
            {
                Console.WriteLine($" Intento 1 falló: {ex.Message}");
                Console.WriteLine("Intentando método alternativo...");

                try
                {// Método alternativo para iniciar ChromeDriver
                    driver = new ChromeDriver(options);
                    Console.WriteLine(" Chrome configurado correctamente (método alternativo)");
                }
                catch (Exception ex2)
                {
                    // Error crítico al iniciar Chrome
                    Console.WriteLine($" Error crítico al iniciar Chrome: {ex2.Message}");
                    Console.WriteLine("\n SOLUCIONES POSIBLES:");
                    Console.WriteLine("1. Verifica que Google Chrome esté instalado");
                    Console.WriteLine("2. Actualiza Chrome a la última versión");
                    Console.WriteLine("3. Reinstala el paquete Selenium.WebDriver.ChromeDriver");
                    Console.WriteLine("   dotnet remove package Selenium.WebDriver.ChromeDriver");
                    Console.WriteLine("   dotnet add package Selenium.WebDriver.ChromeDriver");
                    throw;
                }
            }

            // Configurar timeouts
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
            driver.Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(30);

            Console.WriteLine($" URL base configurada: {baseUrl}");

            // Pequeño delay para asegurar que Chrome esté listo
            Thread.Sleep(1000);
        }

        // Limpiar después de cada prueba
        public virtual void TearDown(string testName, bool passed)
        {
            if (driver != null)
            {
                try
                {
                    Console.WriteLine($" Tomando captura de pantalla: {testName}");
                    TakeScreenshot(testName, passed ? "PASS" : "FAIL");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($" No se pudo tomar screenshot: {ex.Message}");
                }

                try
                {
                    driver.Quit();
                    driver.Dispose();
                    Console.WriteLine(" Navegador cerrado");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($" Error al cerrar navegador: {ex.Message}");
                }
            }
        }

        // Tomar captura de pantalla y guardarla en carpeta Screenshots
        protected void TakeScreenshot(string testName, string status)
        {
            try
            {
                if (driver == null)
                {
                    Console.WriteLine(" No se puede tomar screenshot: driver es null");
                    return;
                }

                // Tomar screenshot y guardarlo
                var screenshot = ((ITakesScreenshot)driver).GetScreenshot();
                string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
                string fileName = $"{testName}_{status}_{timestamp}.png";

                string directoryPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Screenshots");

                if (!Directory.Exists(directoryPath))
                {
                    Directory.CreateDirectory(directoryPath);
                }

                string filePath = Path.Combine(directoryPath, fileName);
                screenshot.SaveAsFile(filePath);

                Console.WriteLine($" Screenshot guardado: {filePath}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($" Error al guardar screenshot: {ex.Message}");
            }
        }

        // Método para hacer login en la aplicación
        protected void Login(string username = "alumno1", string password = "P@ssw0rd!")
        {
            Console.WriteLine($" Iniciando sesión como: {username}");

            // Pequeño delay para asegurar que Chrome esté listo
            Thread.Sleep(2000);

            try
            {
                // Navegar a la página de login
                Console.WriteLine($" Navegando a: {baseUrl}/Account/Login");
                driver.Navigate().GoToUrl($"{baseUrl}/Account/Login");

                // Esperar a que la página cargue completamente
                wait.Until(d => d.FindElement(By.Id("username")));

                Console.WriteLine("  Ingresando credenciales...");

                // Rellenar campos de usuario y contraseña
                var usernameField = driver.FindElement(By.Id("username"));
                usernameField.Clear();
                usernameField.SendKeys(username);

                // Rellenar campo de contraseña
                var passwordField = driver.FindElement(By.Id("password"));
                passwordField.Clear();
                passwordField.SendKeys(password);

                Console.WriteLine("  Haciendo click en Login...");

                // Buscar el botón por texto y hacer click
                var loginButton = driver.FindElement(By.XPath("//button[contains(text(), 'Ingresar')]"));
                loginButton.Click();

                // Esperar redirección al Dashboard o Accounts
                Console.WriteLine(" Esperando redirección...");
                wait.Until(d => d.Url.Contains("Dashboard") || d.Url.Contains("Accounts"));

                Console.WriteLine(" Login exitoso");
            }
            catch (WebDriverTimeoutException ex)
            {
                // Timeout esperando elementos o redirección
                Console.WriteLine($" Timeout al intentar hacer login: {ex.Message}");
                Console.WriteLine($"URL actual: {driver.Url}");
                throw new Exception("No se pudo completar el login. Verifica que la aplicación esté corriendo.");
            }
            catch (NoSuchElementException ex)
            {
                Console.WriteLine($" No se encontró elemento en la página: {ex.Message}");
                Console.WriteLine($"URL actual: {driver.Url}");

                // Mostrar qué elementos SÍ existen
                try
                {
                    // Inputs
                    var allButtons = driver.FindElements(By.TagName("button"));
                    Console.WriteLine($"Botones encontrados: {allButtons.Count}");
                    foreach (var btn in allButtons)
                    {
                        Console.WriteLine($"  - Texto: '{btn.Text}', Type: '{btn.GetAttribute("type")}'");
                    }
                }
                catch { }

                throw new Exception("La página de login no tiene los elementos esperados.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($" Error durante login: {ex.Message}");
                Console.WriteLine($"URL actual: {driver.Url}");
                throw;
            }
        }

        // Método auxiliar para esperar a que un elemento esté visible
        protected IWebElement WaitForElement(By locator, int timeoutInSeconds = 10)
        {
            try
            {
                var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeoutInSeconds));
                return wait.Until(d => {
                    var element = d.FindElement(locator);
                    return element.Displayed ? element : null;
                });
            }
            catch (WebDriverTimeoutException)
            {
                Console.WriteLine($" Timeout esperando elemento: {locator}");
                throw;
            }
        }

        // Método auxiliar para hacer click seguro
        protected void SafeClick(By locator)
        {
            try
            {
                var element = WaitForElement(locator);
                element.Click();
            }
            catch (Exception ex)
            {
                Console.WriteLine($" Click falló, intentando con JavaScript: {ex.Message}");
                var element = driver.FindElement(locator);
                ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", element);
            }
        }

        // Método para verificar si la aplicación está corriendo
        protected bool VerificarConexion()
        {
            try
            {
                Console.WriteLine($" Verificando conexión a: {baseUrl}");
                driver.Navigate().GoToUrl(baseUrl);
                Thread.Sleep(2000);

                bool conectado = !driver.PageSource.Contains("ERR_CONNECTION_REFUSED");

                if (conectado)
                {
                    Console.WriteLine(" Conexión exitosa");
                }
                else
                {
                    Console.WriteLine(" No se pudo conectar a la aplicación");
                }

                return conectado;
            }
            catch (Exception ex)
            {
                Console.WriteLine($" Error verificando conexión: {ex.Message}");
                return false;
            }
        }

        // Método para scroll a un elemento específico
        protected void ScrollToElement(IWebElement element)
        {
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", element);
            Thread.Sleep(500);
        }

        // Método para esperar y hacer click en un elemento
        protected void WaitAndClick(By locator, int timeoutInSeconds = 10)
        {
            var element = WaitForElement(locator, timeoutInSeconds);
            ScrollToElement(element);
            SafeClick(locator);
        }
    }
}