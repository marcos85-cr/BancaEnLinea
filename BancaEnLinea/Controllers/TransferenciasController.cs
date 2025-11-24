
using Microsoft.AspNetCore.Mvc;
using BancaEnLinea.Services;

namespace BancaEnLinea.Controllers;

public class TransferenciasController(AccountsService accounts, TransferenciasService tx) : Controller
{
    private readonly AccountsService _accounts = accounts;
    private readonly TransferenciasService _tx = tx;

    [HttpGet]
    public IActionResult Create()
    {
        var username = HttpContext.Session.GetString("user")!;
        ViewBag.Cuentas = _accounts.GetAccountsFor(username).ToList();
        return View();
    }

    [HttpPost]
    public IActionResult Create(string cuentaOrigenId, string cuentaDestinoId, decimal monto)
    {
        var username = HttpContext.Session.GetString("user")!;
        var (ok, error, _) = _tx.Ejecutar(username, cuentaOrigenId, cuentaDestinoId, monto);
        if (!ok)
        {
            ViewBag.Error = error;
            ViewBag.Cuentas = _accounts.GetAccountsFor(username).ToList();
            return View();
        }
        TempData["msg"] = "Transferencia realizada con éxito";
        return RedirectToAction("Overview", "Accounts");
    }
}
