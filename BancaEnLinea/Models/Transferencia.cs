
namespace BancaEnLinea.Models;

public class Transferencia
{
    public string Id { get; set; } = Guid.NewGuid().ToString("N");
    public string CuentaOrigenId { get; set; } = "";
    public string CuentaDestinoId { get; set; } = "";
    public decimal Monto { get; set; }
    public DateTime Fecha { get; set; } = DateTime.UtcNow;
    public string Estado { get; set; } = "OK"; // OK | RECHAZADA
    public string MotivoRechazo { get; set; } = "";
    public string OwnerUsername { get; set; } = "";
}
