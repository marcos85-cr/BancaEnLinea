
using System.Text.RegularExpressions;
using BancaEnLinea.Models;

namespace BancaEnLinea.Services;

public class BeneficiariosService(InMemoryStore store)
{
    private readonly InMemoryStore _store = store;

    public IEnumerable<Beneficiario> List(string username)
        => _store.Beneficiarios.Where(b => b.OwnerUsername == username);

    public Beneficiario? Get(string id, string username)
        => _store.Beneficiarios.FirstOrDefault(b => b.Id == id && b.OwnerUsername == username);

    public (bool ok, string? error, Beneficiario? saved) Create(string username, string alias, string banco, string numeroCuenta)
    {
        if (!Regex.IsMatch(numeroCuenta ?? "", @"^[0-9\-]{10,22}$"))
            return (false, "Cuenta inválida", null);
        var b = new Beneficiario { OwnerUsername = username, Alias = alias, Banco = banco, NumeroCuenta = numeroCuenta };
        _store.Beneficiarios.Add(b);
        return (true, null, b);
    }

    public (bool ok, string? error) Update(string id, string username, string alias, string banco, string numeroCuenta)
    {
        var b = Get(id, username);
        if (b is null) return (false, "No encontrado");
        if (!Regex.IsMatch(numeroCuenta ?? "", @"^[0-9\-]{10,22}$"))
            return (false, "Cuenta inválida");
        b.Alias = alias; b.Banco = banco; b.NumeroCuenta = numeroCuenta;
        return (true, null);
    }

    public (bool ok, string? error) Delete(string id, string username)
    {
        var b = Get(id, username);
        if (b is null) return (false, "No encontrado");
        _store.Beneficiarios.Remove(b);
        return (true, null);
    }
}
