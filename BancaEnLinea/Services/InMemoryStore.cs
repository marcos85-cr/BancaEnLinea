
using BancaEnLinea.Models;

namespace BancaEnLinea.Services;

public class InMemoryStore
{
    public List<User> Users { get; } = new();
    public List<Account> Accounts { get; } = new();
    public List<Beneficiario> Beneficiarios { get; } = new();
    public List<Transferencia> Transferencias { get; } = new();
    public List<Pago> Pagos { get; } = new();
    public List<Movimiento> Movimientos { get; } = new();

    public decimal LimiteDiario { get; } = 200000m; // ₡200.000 simulado

    public InMemoryStore()
    {
        // Seed
        Users.Add(new User
        {
            Username = "alumno1",
            Password = "P@ssw0rd!",
            FullName = "Alumno Demo"
        });

        Accounts.Add(new Account { Name = "Cuenta A", Balance = 500000m, OwnerUsername = "alumno1" });
        Accounts.Add(new Account { Name = "Cuenta B", Balance = 150000m, OwnerUsername = "alumno1" });
    }
}
