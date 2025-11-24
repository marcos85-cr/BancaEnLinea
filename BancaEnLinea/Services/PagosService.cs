
using BancaEnLinea.Models;

namespace BancaEnLinea.Services;

public class PagosService(InMemoryStore store, AccountsService accounts)
{
    private readonly InMemoryStore _store = store;
    private readonly AccountsService _accounts = accounts;

    public (bool ok, string? error, Pago? pago) Pagar(string username, string cuentaOrigenId, string servicio, string referencia, decimal monto)
    {
        if (monto <= 0) return (false, "Monto inválido", null);
        var cuenta = _accounts.GetById(cuentaOrigenId, username);
        if (cuenta is null) return (false, "Cuenta no encontrada", null);
        if (cuenta.Balance < monto) return (false, "Fondos insuficientes", null);

        cuenta.Balance -= monto;
        var p = new Pago
        {
            OwnerUsername = username,
            Servicio = servicio,
            Referencia = referencia,
            Monto = monto,
            Fecha = DateTime.UtcNow
        };
        _store.Pagos.Add(p);
        _accounts.AddMovimiento(username, cuenta.Id, "Pago", $"{servicio} ({referencia})", -monto);
        return (true, null, p);
    }
}
