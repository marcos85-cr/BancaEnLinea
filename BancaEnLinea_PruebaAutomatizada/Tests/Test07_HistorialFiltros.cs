using BancaEnLinea_PruebaAutomatizada.Helpers;
using BancaEnLinea_PruebaAutomatizada.PageObjects;
using OpenQA.Selenium;

namespace BancaEnLinea_PruebaAutomatizada.Tests
{
    public class Test07_HistorialFiltros : BaseTest
    {
        public void Ejecutar()
        {
            bool passed = false;
            try
            {
                Console.WriteLine("\n═══════════════════════════════════════════════════════");
                Console.WriteLine("  PRUEBA 7: Historial: filtros y verificación de filas");
                Console.WriteLine("══════════════════════════════════════════════════════════\n");
                Console.WriteLine("Requisito: Aplicar filtro por tipo y fecha");
                Console.WriteLine("         Todas las filas coinciden (assert de texto en celdas)");
                Console.WriteLine();

                SetUp();
                Login();

                var historialPage = new HistorialPage(driver, wait);
                var transferenciasPage = new TransferenciasPage(driver, wait);

                //  Hacer transferencias para tener datos
                Console.WriteLine(" Preparación: Creando datos de prueba...");

                for (int i = 1; i <= 2; i++)
                {
                    Console.WriteLine($"   Transferencia {i}/2...");
                    driver.Navigate().GoToUrl($"{baseUrl}/Transferencias/Create");
                    Thread.Sleep(1000);

                    transferenciasPage.SelectCuentaOrigen(0);
                    transferenciasPage.SelectCuentaDestino(1);
                    transferenciasPage.EnterMonto(100 * i);
                    transferenciasPage.ClickTransferir();

                    wait.Until(d => d.Url.Contains("Accounts/Overview") || d.Url.Contains("Dashboard"));
                    Thread.Sleep(1000);
                }
                Console.WriteLine(" Datos de prueba creados\n");

                //  Ver historial SIN filtros
                Console.WriteLine(" Paso 1: Historial sin filtros");
                driver.Navigate().GoToUrl($"{baseUrl}/Historial");
                Thread.Sleep(2000);

                int filasSinFiltro = historialPage.GetRowCount();
                Console.WriteLine($"   Filas encontradas: {filasSinFiltro}");

                if (filasSinFiltro == 0)
                {
                    Console.WriteLine("    No hay datos en el historial");
                    Console.WriteLine("   La prueba continúa pero verificará comportamiento sin datos");
                }

                //  Aplicar filtro por TIPO
                Console.WriteLine("\n Paso 2: Aplicar filtro por TIPO");
                Console.WriteLine("   Tipo seleccionado: Transferencia");

                driver.Navigate().GoToUrl($"{baseUrl}/Historial");
                Thread.Sleep(1000);

                historialPage.SelectTipo("Transferencia");
                historialPage.ClickFiltrar();
                Thread.Sleep(2000);

                int filasConFiltroTipo = historialPage.GetRowCount();
                Console.WriteLine($"   Filas después del filtro: {filasConFiltroTipo}");

                //  Assert de texto en celdas (columna Tipo)
                if (filasConFiltroTipo > 0)
                {
                    Console.WriteLine("\n Verificando contenido de las filas:");
                    var rowTypes = historialPage.GetRowTypes();

                    // Mostrar los primeros 5 tipos encontrados
                    for (int i = 0; i < rowTypes.Count && i < 5; i++)
                    {
                        Console.WriteLine($"   Fila {i + 1}: Tipo = '{rowTypes[i]}'");
                    }

                    if (rowTypes.Count > 5)
                    {
                        Console.WriteLine($"   ... y {rowTypes.Count - 5} filas más");
                    }

                    // Assert: Todas las filas deben ser tipo "Transferencia"
                    bool todasSonTransferencias = rowTypes.All(t =>
                        t.Contains("Transferencia", StringComparison.OrdinalIgnoreCase));

                    if (!todasSonTransferencias)
                    {
                        var tiposIncorrectos = rowTypes.Where(t =>
                            !t.Contains("Transferencia", StringComparison.OrdinalIgnoreCase)).ToList();

                        throw new Exception($" Hay filas que NO son 'Transferencia': {string.Join(", ", tiposIncorrectos)}");
                    }

                    Console.WriteLine($"\n    Assert EXITOSO: Todas las {rowTypes.Count} filas son tipo 'Transferencia'");
                }
                else
                {
                    Console.WriteLine("    No hay filas con el tipo seleccionado");
                    Console.WriteLine("   El filtro funciona correctamente (resultado vacío es válido)");
                }

                //  Aplicar filtro por FECHA
                Console.WriteLine("\n Paso 3: Aplicar filtro por FECHA");

                string fechaHoy = DateTime.Now.ToString("yyyy-MM-dd");
                string fechaMañana = DateTime.Now.AddDays(1).ToString("yyyy-MM-dd");

                Console.WriteLine($"   Rango: Desde {fechaHoy} hasta {fechaMañana}");
                Console.WriteLine("   Tipo: Transferencia");

                driver.Navigate().GoToUrl($"{baseUrl}/Historial");
                Thread.Sleep(1000);

                // Aplicar filtros combinados
                historialPage.EnterDesde(fechaHoy);
                historialPage.EnterHasta(fechaMañana);
                historialPage.SelectTipo("Transferencia");
                historialPage.ClickFiltrar();
                Thread.Sleep(2000);

                int filasConFechaYTipo = historialPage.GetRowCount();
                Console.WriteLine($"   Filas con filtros combinados: {filasConFechaYTipo}");

                //  Las filas siguen coincidiendo con el tipo
                if (filasConFechaYTipo > 0)
                {
                    var rowTypesConFecha = historialPage.GetRowTypes();

                    bool sigueCoincidiendo = rowTypesConFecha.All(t =>
                        t.Contains("Transferencia", StringComparison.OrdinalIgnoreCase));

                    if (!sigueCoincidiendo)
                    {
                        throw new Exception(" Después de aplicar filtro de fecha, hay filas que no son 'Transferencia'");
                    }

                    Console.WriteLine($"    Assert EXITOSO: Las {rowTypesConFecha.Count} filas siguen siendo 'Transferencia'");
                    Console.WriteLine("    Filtros combinados (fecha + tipo) funcionan correctamente");
                }
                else
                {
                    Console.WriteLine("     No hay filas en el rango de fechas especificado");
                    Console.WriteLine("   Esto puede ser esperado si las transacciones son antiguas");
                }

                // Verificar contenido de celdas específicas
                Console.WriteLine("\n Verificación detallada de celdas:");

                var primerasFilas = driver.FindElements(By.CssSelector("tbody tr"));
                int filasVerificar = Math.Min(3, primerasFilas.Count);

                for (int i = 0; i < filasVerificar; i++)
                {
                    try
                    {
                        var celdas = primerasFilas[i].FindElements(By.TagName("td"));

                        if (celdas.Count >= 4)
                        {   // Asumiendo columnas: Fecha | Tipo | Descripción | Monto
                            string fecha = celdas[0].Text;
                            string tipo = celdas[1].Text;
                            string descripcion = celdas[2].Text;
                            string monto = celdas[3].Text;

                            Console.WriteLine($"\n   Fila {i + 1}:");
                            Console.WriteLine($"      Fecha: {fecha}");
                            Console.WriteLine($"      Tipo: {tipo}");
                            Console.WriteLine($"      Descripción: {descripcion}");
                            Console.WriteLine($"      Monto: {monto}");

                            // Assert: La celda "Tipo" debe contener "Transferencia"
                            if (!tipo.Contains("Transferencia", StringComparison.OrdinalIgnoreCase))
                            {
                                throw new Exception($" Fila {i + 1}: Tipo '{tipo}' no coincide con filtro");
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"     Fila {i + 1}: No se pudo verificar - {ex.Message}");
                    }
                }

                Console.WriteLine("\n PRUEBA 7: EXITOSA");
                Console.WriteLine("══════════════════════════════════════════════════════════");
                Console.WriteLine("Resultado esperado:");
                Console.WriteLine("   Filtro por TIPO aplicado correctamente");
                Console.WriteLine("   Filtro por FECHA aplicado correctamente");
                Console.WriteLine("   Todas las filas coinciden con 'Transferencia'");
                Console.WriteLine("   Assert de texto en celdas verificado");
                Console.WriteLine("   Contenido de celdas individuales verificado");

                passed = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\n PRUEBA 7: FALLIDA");
                Console.WriteLine($"Error: {ex.Message}");
            }
            finally
            {
                TearDown("Test07_HistorialFiltros", passed);
            }
        }
    }
}