using BancaEnLinea_PruebaAutomatizada.Helpers;
using BancaEnLinea_PruebaAutomatizada.PageObjects;

namespace BancaEnLinea_PruebaAutomatizada.Tests
{
    // Clase de prueba para el login y verificación del dashboard
    public class Test01_Login : BaseTest
    {
        // Método principal que ejecuta la prueba
        public void Ejecutar()
        {
            bool passed = false;
            try
            {
                Console.WriteLine("\n═════════════════════════════════════════════════════════════════");
                Console.WriteLine("  PRUEBA 1: Login y verificación de nombre de usuario en Dashboard");
                Console.WriteLine("═══════════════════════════════════════════════════════════════════\n");

                SetUp();

                var loginPage = new LoginPage(driver, wait);
                var dashboardPage = new DashboardPage(driver, wait);

                // Navegación a la página de login
                Console.WriteLine($" Navegando a: {baseUrl}/Account/Login");
                driver.Navigate().GoToUrl($"{baseUrl}/Account/Login");

                // Verificación de la URL
                Console.WriteLine("  Ingresando credenciales...");
                loginPage.EnterUsername("alumno1");
                loginPage.EnterPassword("P@ssw0rd!");
                loginPage.ClickLogin();

                // Espera a la redirección al dashboard
                Console.WriteLine(" Esperando redirección...");
                wait.Until(d => d.Url.Contains("Dashboard"));

                // Obtención del mensaje de bienvenida
                var welcomeMessage = dashboardPage.GetWelcomeMessage();
                Console.WriteLine($" Mensaje de bienvenida: {welcomeMessage}");

                // Verificaciones
                if (!driver.Url.Contains("Dashboard"))
                {
                    throw new Exception(" No se redirigió al Dashboard");
                }

                if (!welcomeMessage.Contains("Alumno1"))
                {
                    throw new Exception(" El nombre de usuario no aparece correctamente");
                }

                // Verificación de los saldos de las cuentas
                Console.WriteLine("\n PRUEBA 1: EXITOSA");
                passed = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\n PRUEBA 1: FALLIDA");
                Console.WriteLine($"Error: {ex.Message}");
            }
            finally
            {
                TearDown("Test01_Login", passed);
            }
        }
    }
}