using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace BancaEnLinea_PruebaAutomatizada.PageObjects
{
    //clase que representa la página de beneficiarios y sus interacciones
    public class BeneficiariosPage
    {
        private readonly IWebDriver driver;
        private readonly WebDriverWait wait;

        private By addButton = By.CssSelector("a[href='/Beneficiarios/Create']");
        private By aliasInput = By.Name("alias");
        private By bancoInput = By.Name("banco");
        private By numeroCuentaInput = By.Name("numeroCuenta");
        private By saveButton = By.CssSelector("button[type='submit']");
        private By errorMessage = By.CssSelector(".alert-danger");
        private By successMessage = By.CssSelector(".alert-success");
        private By tableRows = By.CssSelector("tbody tr");
        private By editButtons = By.CssSelector("a.btn-outline-secondary");
        private By deleteButtons = By.CssSelector("button.btn-outline-danger");

        //constructor que inicializa el controlador web y la espera explícita
        public BeneficiariosPage(IWebDriver driver, WebDriverWait wait)
        {
            this.driver = driver;
            this.wait = wait;
        }

        //hace clic en el botón agregar nuevo beneficiario
        public void ClickAddBeneficiario()
        {
            wait.Until(d => d.FindElement(addButton));
            driver.FindElement(addButton).Click();
        }

        //rellena el formulario de beneficiario con los datos proporcionados
        public void FillBeneficiarioForm(string alias, string banco, string numeroCuenta)
        {
            wait.Until(d => d.FindElement(aliasInput));

            driver.FindElement(aliasInput).Clear();
            driver.FindElement(aliasInput).SendKeys(alias);

            driver.FindElement(bancoInput).Clear();
            driver.FindElement(bancoInput).SendKeys(banco);

            driver.FindElement(numeroCuentaInput).Clear();
            driver.FindElement(numeroCuentaInput).SendKeys(numeroCuenta);
        }

        //hace clic en el botón guardar para agregar o editar un beneficiario
        public void ClickSave()
        {
            driver.FindElement(saveButton).Click();
        }

        //obtiene el mensaje de error si existe después de intentar agregar o editar un beneficiario
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
        //obtiene el mensaje de éxito después de agregar o editar un beneficiario
        public string GetSuccessMessage()
        {
            try
            {
                wait.Until(d => d.FindElement(successMessage));
                return driver.FindElement(successMessage).Text;
            }
            catch
            {
                return string.Empty;
            }
        }
        //verifica si un beneficiario con el alias dado existe en la tabla
        public bool BeneficiarioExists(string alias)
        {
            try
            {
                var rows = driver.FindElements(tableRows);
                return rows.Any(row => row.Text.Contains(alias));
            }
            catch
            {
                return false;
            }
        }

        //selecciona el primer beneficiario y hace clic en editar
        public void ClickEditFirstBeneficiario()
        {
            wait.Until(d => d.FindElements(editButtons).Count > 0);
            driver.FindElements(editButtons).First().Click();
        }

        //selecciona el primer beneficiario y hace clic en eliminar
        public void ClickDeleteFirstBeneficiario()
        {
            wait.Until(d => d.FindElements(deleteButtons).Count > 0);
            driver.FindElements(deleteButtons).First().Click();
            driver.SwitchTo().Alert().Accept();
        }

        //cuenta el número de beneficiarios en la tabla
        public int GetBeneficiariosCount()
        {
            try
            {
                return driver.FindElements(tableRows).Count;
            }
            catch
            {
                return 0;
            }
        }
    }
}
