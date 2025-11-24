
using Microsoft.AspNetCore.Mvc;
using BancaEnLinea.Services;

namespace BancaEnLinea.Controllers;

public class DashboardController(AccountsService accounts) : Controller
{
    private readonly AccountsService _accounts = accounts;

    public IActionResult Index()
    {
        var username = HttpContext.Session.GetString("user")!;
        var fullName = HttpContext.Session.GetString("fullName") ?? username;
        var cuentas = _accounts.GetAccountsFor(username).ToList();
        ViewBag.FullName = fullName;
        return View(cuentas);
    }
}
