
namespace BancaEnLinea.Models;

public class Pago
{
    public string Id { get; set; } = Guid.NewGuid().ToString("N");
    public string Servicio { get; set; } = "";
    public string Referencia { get; set; } = "";
    public decimal Monto { get; set; }
    public DateTime Fecha { get; set; } = DateTime.UtcNow;
    public string OwnerUsername { get; set; } = "";
}
