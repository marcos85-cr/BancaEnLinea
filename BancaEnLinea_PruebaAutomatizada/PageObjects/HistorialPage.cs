using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace BancaEnLinea_PruebaAutomatizada.PageObjects
{
    public class HistorialPage
    {
        private readonly IWebDriver driver;
        private readonly WebDriverWait wait;

        // localizadores de elementos
        private By desdeInput = By.Name("desde");
        private By hastaInput = By.Name("hasta");
        private By tipoSelect = By.Name("tipo");
        private By filtrarButton = By.CssSelector("button[type='submit']");
        private By exportButton = By.CssSelector("a[href='/Historial/Export']");
        private By tableRows = By.CssSelector("tbody tr");

        // constructor que inicializa el controlador web y la espera explícita
        public HistorialPage(IWebDriver driver, WebDriverWait wait)
        {
            this.driver = driver;
            this.wait = wait;
        }

        public void SelectTipo(string tipo)
        {
            // Esperar a que el select esté presente
            wait.Until(d => d.FindElement(tipoSelect));
            var select = new SelectElement(driver.FindElement(tipoSelect));
            select.SelectByText(tipo);
        }

        public void ClickFiltrar()
        {
            driver.FindElement(filtrarButton).Click(); // Hacer clic en el botón Filtrar
        }

        public void ClickExportar()
        {
            driver.FindElement(exportButton).Click();
        }
        // Método para obtener el número de filas en la tabla de historial
        public int GetRowCount()
        {
            try
            {
                // Esperar a que las filas de la tabla estén presentes
                wait.Until(d => d.FindElements(tableRows).Count > 0);
                return driver.FindElements(tableRows).Count;
            }
            catch
            {
                return 0;
            }
        }
        // Método para obtener
        public List<string> GetRowTypes()
        {
            // Esperar a que las filas de la tabla estén presentes
            var rows = driver.FindElements(tableRows);
            return rows.Select(row => {
                var cells = row.FindElements(By.TagName("td")); // Obtener las celdas de la fila
                return cells.Count > 1 ? cells[1].Text : "";
            }).ToList();
        }
    }
}
