using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace BancaEnLinea_PruebaAutomatizada.PageObjects
{
    public class LoginPage
    {   // Actualizado
        private readonly IWebDriver driver;
        private readonly WebDriverWait wait;

        // Localizadores - Actualizados
        private By usernameInput = By.Id("username");  // Cambiado
        private By passwordInput = By.Id("password");  // Cambiado
        private By loginButton = By.XPath("//button[contains(text(), 'Ingresar')]");  
        private By errorMessage = By.CssSelector(".alert-danger");

        // Constructor
        public LoginPage(IWebDriver driver, WebDriverWait wait)
        {
            this.driver = driver;
            this.wait = wait;
        }

        // Métodos de interacción - Actualizados
        public void EnterUsername(string username)
        {
            wait.Until(d => d.FindElement(usernameInput));
            var element = driver.FindElement(usernameInput);
            element.Clear();
            element.SendKeys(username);
        }

        // Métodos de interacción - Actualizados
        public void EnterPassword(string password)
        {
            var element = driver.FindElement(passwordInput);
            element.Clear();
            element.SendKeys(password);
        }

        
        public void ClickLogin()
        {
            // Esperar hasta que el botón de login sea clickeable
            var button = wait.Until(d => d.FindElement(loginButton));
            button.Click();
        }

        // Método para obtener el mensaje de error - Actualizado
        public string GetErrorMessage()
        {
            try
            {
                return driver.FindElement(errorMessage).Text;
            }
            catch
            {
                return string.Empty;
            }
        }
    }
}