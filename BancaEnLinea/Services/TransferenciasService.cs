
using BancaEnLinea.Models;

namespace BancaEnLinea.Services;

public class TransferenciasService(InMemoryStore store, AccountsService accounts)
{
    private readonly InMemoryStore _store = store;
    private readonly AccountsService _accounts = accounts;

    private decimal TotalTransferidoHoy(string username)
    {
        var hoy = DateTime.UtcNow.Date;
        return _store.Transferencias
            .Where(t => t.OwnerUsername == username && t.Fecha.Date == hoy && t.Estado == "OK")
            .Sum(t => t.Monto);
    }

    public (bool ok, string? error, Transferencia? tx) Ejecutar(string username, string cuentaOrigenId, string cuentaDestinoId, decimal monto)
    {
        if (monto <= 0) return (false, "Monto inválido", null);

        var origen = _accounts.GetById(cuentaOrigenId, username);
        var destino = _accounts.GetById(cuentaDestinoId, username);
        if (origen is null || destino is null) return (false, "Cuenta no encontrada", null);

        if (origen.Balance < monto)
            return (false, "Fondos insuficientes", null);

        var totalHoy = TotalTransferidoHoy(username) + monto;
        if (totalHoy > _store.LimiteDiario)
            return (false, "Límite diario excedido", null);

        origen.Balance -= monto;
        destino.Balance += monto;

        var t = new Transferencia
        {
            OwnerUsername = username,
            CuentaOrigenId = cuentaOrigenId,
            CuentaDestinoId = cuentaDestinoId,
            Monto = monto,
            Estado = "OK",
            Fecha = DateTime.UtcNow
        };
        _store.Transferencias.Add(t);

        _accounts.AddMovimiento(username, cuentaOrigenId, "Transferencia", $"A {destino.Name}", -monto);
        _accounts.AddMovimiento(username, cuentaDestinoId, "Transferencia", $"Desde {origen.Name}", monto);

        return (true, null, t);
    }
}
