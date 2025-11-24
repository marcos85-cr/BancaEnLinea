
namespace BancaEnLinea.Models;

public class Movimiento
{
    public string Id { get; set; } = Guid.NewGuid().ToString("N");
    public DateTime Fecha { get; set; } = DateTime.UtcNow;
    public string Tipo { get; set; } = ""; // Transferencia, Pago
    public string Descripcion { get; set; } = "";
    public decimal Monto { get; set; }
    public string CuentaAfectadaId { get; set; } = "";
    public string OwnerUsername { get; set; } = "";
}
