using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace BancaEnLinea_PruebaAutomatizada.PageObjects
{
    public class LoginPage
    {
        private readonly IWebDriver driver;
        private readonly WebDriverWait wait;

        // localizadores de elementos
        private By usernameInput = By.Name("username");
        private By passwordInput = By.Name("password");
        private By loginButton = By.CssSelector("button.btn-primary");
        private By errorMessage = By.CssSelector(".alert-danger");

        // constructor
        public LoginPage(IWebDriver driver, WebDriverWait wait)
        {
            this.driver = driver;
            this.wait = wait;
        }

        // métodos para interactuar con la página
        public void EnterUsername(string username)
        {
            wait.Until(d => d.FindElement(usernameInput));
            var element = driver.FindElement(usernameInput);
            element.Clear();
            element.SendKeys(username);
        }

        // método para ingresar la contraseña
        public void EnterPassword(string password)
        {
            var element = driver.FindElement(passwordInput);
            element.Clear();
            element.SendKeys(password);
        }

        // método para hacer clic en el botón de inicio de sesión
        public void ClickLogin()
        {
            var button = wait.Until(d => d.FindElement(loginButton));
            button.Click();
        }

        // método para obtener el mensaje de error
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