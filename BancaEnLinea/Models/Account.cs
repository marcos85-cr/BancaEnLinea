
namespace BancaEnLinea.Models;

public class Account
{
    public string Id { get; set; } = Guid.NewGuid().ToString("N");
    public string Name { get; set; } = "";
    public decimal Balance { get; set; }
    public string OwnerUsername { get; set; } = "";
}
