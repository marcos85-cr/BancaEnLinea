
using BancaEnLinea.Models;

namespace BancaEnLinea.Services;

public class AccountsService(InMemoryStore store)
{
    private readonly InMemoryStore _store = store;

    public IEnumerable<Account> GetAccountsFor(string username)
        => _store.Accounts.Where(a => a.OwnerUsername == username);

    public Account? GetById(string id, string username)
        => _store.Accounts.FirstOrDefault(a => a.Id == id && a.OwnerUsername == username);

    public void AdjustBalance(string id, decimal delta)
    {
        var acc = _store.Accounts.First(a => a.Id == id);
        acc.Balance += delta;
    }

    public void AddMovimiento(string username, string cuentaId, string tipo, string desc, decimal monto)
    {
        _store.Movimientos.Add(new Movimiento
        {
            OwnerUsername = username,
            CuentaAfectadaId = cuentaId,
            Tipo = tipo,
            Descripcion = desc,
            Monto = monto,
            Fecha = DateTime.UtcNow
        });
    }
}
