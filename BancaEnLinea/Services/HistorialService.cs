
using System.Text;
using BancaEnLinea.Models;

namespace BancaEnLinea.Services;

public class HistorialService(InMemoryStore store)
{
    private readonly InMemoryStore _store = store;

    public IEnumerable<Movimiento> Filtrar(string username, DateTime? desde, DateTime? hasta, string? tipo)
    {
        var q = _store.Movimientos.Where(m => m.OwnerUsername == username);
        if (desde is not null) q = q.Where(m => m.Fecha >= desde.Value);
        if (hasta is not null) q = q.Where(m => m.Fecha <= hasta.Value);
        if (!string.IsNullOrWhiteSpace(tipo)) q = q.Where(m => m.Tipo.Equals(tipo, StringComparison.OrdinalIgnoreCase));
        return q.OrderByDescending(m => m.Fecha);
    }

    public (byte[] data, string fileName) ExportarCsv(IEnumerable<Movimiento> movimientos)
    {
        var sb = new StringBuilder();
        sb.AppendLine("Fecha,Tipo,Descripcion,Monto");
        foreach (var m in movimientos)
        {
            sb.AppendLine($"{m.Fecha:yyyy-MM-dd HH:mm:ss},{m.Tipo},{m.Descripcion},{m.Monto}");
        }
        var bytes = Encoding.UTF8.GetBytes(sb.ToString());
        var name = $"historial_{DateTime.UtcNow:yyyyMMddHHmmss}.csv";
        return (bytes, name);
    }
}
