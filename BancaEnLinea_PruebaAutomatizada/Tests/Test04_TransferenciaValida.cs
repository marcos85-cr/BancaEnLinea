using BancaEnLinea_PruebaAutomatizada.Helpers;
using BancaEnLinea_PruebaAutomatizada.PageObjects;

namespace BancaEnLinea_PruebaAutomatizada.Tests
{
    public class Test04_TransferenciaValida : BaseTest
    {
        public void Ejecutar()
        {
            bool passed = false;
            try
            {
                Console.WriteLine("\n═════════════════════════════════════════════════════════════════");
                Console.WriteLine("  PRUEBA 4: Transferencia válida (actualización visual de saldos)");
                Console.WriteLine("════════════════════════════════════════════════════════════════════\n");
                Console.WriteLine("Requisito: Saldos visibles actualizados (resta/suma exactas)");
                Console.WriteLine();

                SetUp();
                Login();

                var dashboardPage = new DashboardPage(driver, wait);
                var transferenciasPage = new TransferenciasPage(driver, wait);
                decimal montoTransferencia = 1000m;

                //  Obtiene saldos iniciales
                Console.WriteLine(" Obteniendo saldos iniciales...");
                driver.Navigate().GoToUrl($"{baseUrl}/Dashboard");
                Thread.Sleep(1000);

                var saldosIniciales = dashboardPage.GetAccountBalances();

                if (saldosIniciales.Count < 2)
                {
                    throw new Exception($" Se esperaban 2 cuentas, se encontraron {saldosIniciales.Count}");
                }

                // Convertir saldos de texto a decimal
                decimal saldoOrigenInicial = ExtraerMonto(saldosIniciales[0]);
                decimal saldoDestinoInicial = ExtraerMonto(saldosIniciales[1]);

                Console.WriteLine($" Saldos iniciales:");
                Console.WriteLine($"   Cuenta Origen (índice 0): ₡{saldoOrigenInicial:N2}");
                Console.WriteLine($"   Cuenta Destino (índice 1): ₡{saldoDestinoInicial:N2}");

                //  Ejecutar transferencia
                Console.WriteLine($"\n Ejecutando transferencia de ₡{montoTransferencia:N2}...");
                driver.Navigate().GoToUrl($"{baseUrl}/Transferencias/Create");
                Thread.Sleep(1000);

                // Completa formulario
                transferenciasPage.SelectCuentaOrigen(0);
                transferenciasPage.SelectCuentaDestino(1);
                transferenciasPage.EnterMonto(montoTransferencia);

                Console.WriteLine("  Formulario completado");
                Console.WriteLine(" Enviando transferencia...");

                transferenciasPage.ClickTransferir();

                // Espera confirmación
                wait.Until(d => d.Url.Contains("Accounts/Overview") || d.Url.Contains("Dashboard"));
                Thread.Sleep(1000); // Esperar a que la página se cargue completamente

                Console.WriteLine(" Transferencia procesada");

                //  Volver a Dashboard y obtener saldos finales
                Console.WriteLine("\n Volviendo al Dashboard...");
                driver.Navigate().GoToUrl($"{baseUrl}/Dashboard");
                Thread.Sleep(1500); // Dar tiempo para que se actualice la vista

                // Obtener saldos finales
                var saldosFinales = dashboardPage.GetAccountBalances();

                if (saldosFinales.Count < 2)
                {
                    throw new Exception($" Error al obtener saldos finales");
                }

                decimal saldoOrigenFinal = ExtraerMonto(saldosFinales[0]);
                decimal saldoDestinoFinal = ExtraerMonto(saldosFinales[1]);

                Console.WriteLine($" Saldos finales:");
                Console.WriteLine($"   Cuenta Origen (índice 0): ₡{saldoOrigenFinal:N2}");
                Console.WriteLine($"   Cuenta Destino (índice 1): ₡{saldoDestinoFinal:N2}");

                // Calcula diferencias
                decimal diferenciaOrigen = saldoOrigenInicial - saldoOrigenFinal;
                decimal diferenciaDestino = saldoDestinoFinal - saldoDestinoInicial;

                Console.WriteLine($"\n Verificando cambios:");
                Console.WriteLine($"   Origen: ₡{saldoOrigenInicial:N2} - ₡{montoTransferencia:N2} = ₡{saldoOrigenInicial - montoTransferencia:N2}");
                Console.WriteLine($"   Actual: ₡{saldoOrigenFinal:N2}");
                Console.WriteLine($"   Diferencia calculada: ₡{diferenciaOrigen:N2}");
                Console.WriteLine();
                Console.WriteLine($"   Destino: ₡{saldoDestinoInicial:N2} + ₡{montoTransferencia:N2} = ₡{saldoDestinoInicial + montoTransferencia:N2}");
                Console.WriteLine($"   Actual: ₡{saldoDestinoFinal:N2}");
                Console.WriteLine($"   Diferencia calculada: ₡{diferenciaDestino:N2}");

                //  Resta exacta en cuenta origen
                if (Math.Abs(diferenciaOrigen - montoTransferencia) > 0.01m)
                {
                    throw new Exception($" Resta incorrecta en origen. Esperado: ₡{montoTransferencia:N2}, Real: ₡{diferenciaOrigen:N2}");
                }
                Console.WriteLine("\n Resta exacta en cuenta origen confirmada");

                //  Suma exacta en cuenta destino
                if (Math.Abs(diferenciaDestino - montoTransferencia) > 0.01m)
                {
                    throw new Exception($" Suma incorrecta en destino. Esperado: ₡{montoTransferencia:N2}, Real: ₡{diferenciaDestino:N2}");
                }
                Console.WriteLine(" Suma exacta en cuenta destino confirmada");

                //  Los saldos cambiaron visualmente
                if (saldoOrigenFinal == saldoOrigenInicial)
                {
                    throw new Exception(" El saldo origen no cambió visualmente");
                }

                if (saldoDestinoFinal == saldoDestinoInicial)
                {
                    throw new Exception(" El saldo destino no cambió visualmente");
                }
                Console.WriteLine("✓ Saldos actualizados visualmente");

                Console.WriteLine("\n PRUEBA 4: EXITOSA");
                Console.WriteLine("════════════════════════════════════════════════════════════════════");
                Console.WriteLine("Resultado esperado:");
                Console.WriteLine($"   Transferencia ejecutada: ₡{montoTransferencia:N2}");
                Console.WriteLine($"   Origen: ₡{saldoOrigenInicial:N2} a ₡{saldoOrigenFinal:N2} (Resta: ₡{diferenciaOrigen:N2})");
                Console.WriteLine($"   Destino: ₡{saldoDestinoInicial:N2} a ₡{saldoDestinoFinal:N2} (Suma: ₡{diferenciaDestino:N2})");
                Console.WriteLine("   Cálculos exactos confirmados");

                passed = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\n PRUEBA 4: FALLIDA");
                Console.WriteLine($"Error: {ex.Message}");
            }
            finally
            {
                TearDown("Test04_TransferenciaValida", passed);
            }
        }

        // Método auxiliar para extraer el monto de un texto como "₡ 500,000.00"
        private decimal ExtraerMonto(string textoSaldo)
        {
            try
            {
                // Remover símbolos de moneda, espacios y comas
                string numeroLimpio = textoSaldo
                    .Replace("₡", "")
                    .Replace("$", "")
                    .Replace(" ", "")
                    .Replace(",", "")
                    .Trim();

                return decimal.Parse(numeroLimpio);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al extraer monto de '{textoSaldo}': {ex.Message}");
            }
        }
    }
}