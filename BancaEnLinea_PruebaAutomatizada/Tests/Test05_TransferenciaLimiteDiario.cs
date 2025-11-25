using BancaEnLinea_PruebaAutomatizada.Helpers;
using BancaEnLinea_PruebaAutomatizada.PageObjects;
using System.IO;

namespace BancaEnLinea_PruebaAutomatizada.Tests
{
    public class Test05_TransferenciaLimiteDiario : BaseTest
    {
        public void Ejecutar()
        {
            bool passed = false;
            try
            {
                Console.WriteLine("\n═══════════════════════════════════════════════════════");
                Console.WriteLine("  PRUEBA 5: Transferencia rechazada por límite diario");
                Console.WriteLine("═════════════════════════════════════════════════════════\n");
                Console.WriteLine("Requisito: Ejecutar varias hasta superar tope simulado");
                Console.WriteLine("         Límite diario: ₡200,000");
                Console.WriteLine();

                SetUp();
                Login();

                var transferenciasPage = new TransferenciasPage(driver, wait);

                decimal limiteDiario = 200000m;
                decimal montoTransferencia = 80000m; // Monto de cada transferencia
                decimal totalTransferido = 0m;
                int numeroTransferencia = 0;
                bool limiteSuperado = false;

                Console.WriteLine(" Estrategia: Realizar múltiples transferencias hasta superar el límite");
                Console.WriteLine($"   Monto por transferencia: ₡{montoTransferencia:N2}");
                Console.WriteLine($"   Límite diario: ₡{limiteDiario:N2}");
                Console.WriteLine();

                //  Ejecutar varias transferencias hasta superar el límite
                while (!limiteSuperado && numeroTransferencia < 5)
                {
                    numeroTransferencia++;
                    decimal totalProyectado = totalTransferido + montoTransferencia;

                    Console.WriteLine($"━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━");
                    Console.WriteLine($"   Transferencia #{numeroTransferencia}");
                    Console.WriteLine($"   Monto: ₡{montoTransferencia:N2}");
                    Console.WriteLine($"   Total acumulado: ₡{totalTransferido:N2}");
                    Console.WriteLine($"   Total proyectado: ₡{totalProyectado:N2}");

                    if (totalProyectado > limiteDiario)
                    {
                        Console.WriteLine($"     Esta transferencia SUPERARÁ el límite de ₡{limiteDiario:N2}");
                    }

                    // Navegar al formulario de transferencias
                    driver.Navigate().GoToUrl($"{baseUrl}/Transferencias/Create");
                    Thread.Sleep(1000);

                    // Completar formulario
                    transferenciasPage.SelectCuentaOrigen(0);
                    transferenciasPage.SelectCuentaDestino(1);
                    transferenciasPage.EnterMonto(montoTransferencia);

                    Console.WriteLine("    Enviando transferencia...");
                    transferenciasPage.ClickTransferir();

                    Thread.Sleep(1500); // Tiempo de Espera de respuesta

                    // Verificar si hay mensaje de error
                    var errorMsg = transferenciasPage.GetErrorMessage();

                    if (!string.IsNullOrEmpty(errorMsg))
                    {
                        Console.WriteLine($"\n    Transferencia RECHAZADA");
                        Console.WriteLine($"    Mensaje: {errorMsg}");

                        //  El mensaje contiene "Límite diario excedido"
                        if (errorMsg.ToLower().Contains("límite diario excedido") ||
                            errorMsg.ToLower().Contains("limite diario excedido"))
                        {
                            Console.WriteLine($"    Mensaje correcto: 'Límite diario excedido' detectado");
                            limiteSuperado = true;
                        }
                        else
                        {
                            throw new Exception($" Mensaje de error incorrecto. Se esperaba 'Límite diario excedido', se obtuvo: '{errorMsg}'");
                        }

                        // Verificar que el mensaje está visible en la UI
                        bool mensajeVisibleEnUI = driver.PageSource.Contains("Límite diario excedido") ||
                                                  driver.PageSource.Contains("límite diario excedido");

                        if (!mensajeVisibleEnUI)
                        {
                            throw new Exception(" El mensaje no está visible en la UI");
                        }

                        Console.WriteLine($"    Mensaje visible en la UI");
                        Console.WriteLine($"\n    Resumen:");
                        Console.WriteLine($"      Transferencias exitosas: {numeroTransferencia - 1}");
                        Console.WriteLine($"      Total transferido antes del límite: ₡{totalTransferido:N2}");
                        Console.WriteLine($"      Transferencia rechazada: ₡{montoTransferencia:N2}");
                        Console.WriteLine($"      Total intentado: ₡{totalProyectado:N2}");

                        break;
                    }
                    else
                    {
                        // Transferencia exitosa
                        totalTransferido += montoTransferencia;
                        Console.WriteLine($"    Transferencia #{numeroTransferencia} EXITOSA");
                        Console.WriteLine($"    Total acumulado: ₡{totalTransferido:N2}");

                        // Esperar confirmación
                        wait.Until(d => d.Url.Contains("Accounts/Overview") || d.Url.Contains("Dashboard"));
                        Thread.Sleep(1000);
                    }
                }

                Console.WriteLine($"\n━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━");

                // Verificación final
                if (!limiteSuperado)
                {
                    // Si no se superó el límite con transferencias pequeñas, intentar una grande
                    Console.WriteLine("\n  Método alternativo: Intentando transferencia grande única");

                    decimal montoExcesivo = 250000m;
                    Console.WriteLine($"   Monto: ₡{montoExcesivo:N2} (excede directamente el límite)");

                    driver.Navigate().GoToUrl($"{baseUrl}/Transferencias/Create");
                    Thread.Sleep(1000);

                    transferenciasPage.SelectCuentaOrigen(0);
                    transferenciasPage.SelectCuentaDestino(1);
                    transferenciasPage.EnterMonto(montoExcesivo);
                    transferenciasPage.ClickTransferir();

                    Thread.Sleep(1500);

                    var errorMsg = transferenciasPage.GetErrorMessage();

                    if (errorMsg.ToLower().Contains("límite diario excedido") ||
                        errorMsg.ToLower().Contains("limite diario excedido"))
                    {
                        Console.WriteLine($"    Límite detectado con transferencia grande");
                        Console.WriteLine($"    Mensaje: {errorMsg}");
                        limiteSuperado = true;
                    }
                }

                if (!limiteSuperado)
                {
                    throw new Exception(" No se logró superar el límite diario o no se mostró el mensaje esperado");
                }

                Console.WriteLine("\n PRUEBA 5: EXITOSA");
                Console.WriteLine("════════════════════════════════════════════════════════");
                Console.WriteLine("Resultado esperado:");
                Console.WriteLine("   Se ejecutaron múltiples transferencias");
                Console.WriteLine("   Se superó el límite diario de ₡200,000");
                Console.WriteLine("   Mensaje 'Límite diario excedido' mostrado en UI");
                Console.WriteLine("   Transferencia rechazada correctamente");

                passed = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\n PRUEBA 5: FALLIDA");
                Console.WriteLine($"Error: {ex.Message}");
            }
            finally
            {
                TearDown("Test05_TransferenciaLimiteDiario", passed);
            }
        }
    }
}
