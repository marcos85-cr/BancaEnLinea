using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace BancaEnLinea_PruebaAutomatizada.PageObjects
{
    //clase que representa la página de dashboard y sus interacciones
    public class DashboardPage
    {
        private readonly IWebDriver driver;
        private readonly WebDriverWait wait;

        private By welcomeMessage = By.CssSelector(".text-muted");
        private By accountCards = By.CssSelector(".card");
        private By accountBalances = By.CssSelector(".card-text.fs-4");

        //constructor que inicializa el controlador web y la espera explícita
        public DashboardPage(IWebDriver driver, WebDriverWait wait)
        {
            this.driver = driver;
            this.wait = wait;
        }
        //obtiene el mensaje de bienvenida del usuario en el dashboard
        public string GetWelcomeMessage()
        {
            wait.Until(d => d.FindElements(welcomeMessage).Count > 0);
            return driver.FindElement(welcomeMessage).Text;
        }

        //obtiene los saldos de las cuentas mostradas en el dashboard
        public List<string> GetAccountBalances()
        {
            wait.Until(d => d.FindElements(accountBalances).Count > 0);
            return driver.FindElements(accountBalances)
                        .Select(e => e.Text)
                        .ToList();
        }
    }
}