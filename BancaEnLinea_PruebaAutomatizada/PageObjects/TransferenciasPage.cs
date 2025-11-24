using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace BancaEnLinea_PruebaAutomatizada.PageObjects
{   // Clase que representa la página de transferencias y sus interacciones
    public class TransferenciasPage
    {
        private readonly IWebDriver driver;
        private readonly WebDriverWait wait;

        private By cuentaOrigenSelect = By.Name("cuentaOrigenId");
        private By cuentaDestinoSelect = By.Name("cuentaDestinoId");
        private By montoInput = By.Name("monto");
        private By transferButton = By.CssSelector("button[type='submit']");
        private By errorMessage = By.CssSelector(".alert-danger");

        // Constructor que inicializa el controlador web y la espera explícita
        public TransferenciasPage(IWebDriver driver, WebDriverWait wait)
        {
            this.driver = driver;
            this.wait = wait;
        }

        // Método para seleccionar la cuenta de origen desde un menú desplegable
        public void SelectCuentaOrigen(int index = 0)
        {
            wait.Until(d => d.FindElement(cuentaOrigenSelect));
            var select = new SelectElement(driver.FindElement(cuentaOrigenSelect));
            select.SelectByIndex(index);
        }

        // Método para seleccionar la cuenta de destino desde un menú desplegable
        public void SelectCuentaDestino(int index = 1)
        {
            var select = new SelectElement(driver.FindElement(cuentaDestinoSelect));
            select.SelectByIndex(index);
        }

        // Método para ingresar el monto de la transferencia
        public void EnterMonto(decimal monto)
        {
            var element = driver.FindElement(montoInput);
            element.Clear();
            element.SendKeys(monto.ToString());
        }

        // Método para hacer clic en el botón transferir
        public void ClickTransferir()
        {
            driver.FindElement(transferButton).Click();
        }

        // Método para obtener el mensaje de error si existe después de intentar realizar una transferencia
        public string GetErrorMessage()
        {
            try
            {
                wait.Until(d => d.FindElement(errorMessage));
                return driver.FindElement(errorMessage).Text;
            }
            catch
            {
                return string.Empty;
            }
        }
    }
}
