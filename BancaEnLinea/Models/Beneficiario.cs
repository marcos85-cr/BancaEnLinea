
namespace BancaEnLinea.Models;

public class Beneficiario
{
    public string Id { get; set; } = Guid.NewGuid().ToString("N");
    public string Alias { get; set; } = "";
    public string Banco { get; set; } = "";
    public string NumeroCuenta { get; set; } = "";
    public string OwnerUsername { get; set; } = "";
}
