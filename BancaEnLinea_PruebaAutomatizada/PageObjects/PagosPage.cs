using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace BancaEnLinea_PruebaAutomatizada.PageObjects
{
    public class PagosPage
    {
        private readonly IWebDriver driver;
        private readonly WebDriverWait wait;

        private By cuentaOrigenSelect = By.Name("cuentaOrigenId");
        private By servicioInput = By.Name("servicio");
        private By referenciaInput = By.Name("referencia");
        private By montoInput = By.Name("monto");
        private By pagarButton = By.CssSelector("button.btn-primary");

        // Constructor que inicializa el controlador web y la espera explícita
        public PagosPage(IWebDriver driver, WebDriverWait wait)
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
        // Método para ingresar el nombre del servicio
        public void EnterServicio(string servicio)
        {
            driver.FindElement(servicioInput).Clear();
            driver.FindElement(servicioInput).SendKeys(servicio);
        }

        // Método para ingresar la referencia del pago
        public void EnterReferencia(string referencia)
        {
            driver.FindElement(referenciaInput).Clear();
            driver.FindElement(referenciaInput).SendKeys(referencia);
        }

        // Método para ingresar el monto del pago
        public void EnterMonto(decimal monto)
        {
            driver.FindElement(montoInput).Clear();
            driver.FindElement(montoInput).SendKeys(monto.ToString());
        }

        // Método para hacer clic en el botón pagar
        public void ClickPagar()
        {
            driver.FindElement(pagarButton).Click();
        }

        // Método para verificar si el comprobante de pago se muestra correctamente
        public bool IsComprobanteDisplayed()
        {
            try
            {
                wait.Until(d => d.Url.Contains("Receipt"));
                return driver.PageSource.Contains("ID de operación");
            }
            catch
            {
                return false;
            }
        }
    }
}
