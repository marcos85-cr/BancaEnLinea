
using Microsoft.AspNetCore.Mvc;
using BancaEnLinea.Services;
using BancaEnLinea.Models;

namespace BancaEnLinea.Controllers;

public class PagosController(AccountsService accounts, PagosService pagos) : Controller
{
    private readonly AccountsService _accounts = accounts;
    private readonly PagosService _pagos = pagos;

    [HttpGet]
    public IActionResult Create()
    {
        var username = HttpContext.Session.GetString("user")!;
        ViewBag.Cuentas = _accounts.GetAccountsFor(username).ToList();
        return View();
    }

    [HttpPost]
    public IActionResult Create(string cuentaOrigenId, string servicio, string referencia, decimal monto)
    {
        var username = HttpContext.Session.GetString("user")!;
        var (ok, error, pago) = _pagos.Pagar(username, cuentaOrigenId, servicio, referencia, monto);
        if (!ok)
        {
            ViewBag.Error = error;
            ViewBag.Cuentas = _accounts.GetAccountsFor(username).ToList();
            return View();
        }
        return RedirectToAction(nameof(Receipt), new { id = pago!.Id });
    }

    public IActionResult Receipt(string id)
    {
        // Simple simulated receipt
        return View(model: id);
    }
}
