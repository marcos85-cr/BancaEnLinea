
using Microsoft.AspNetCore.Mvc;
using BancaEnLinea.Services;

namespace BancaEnLinea.Controllers;

public class AccountsController(AccountsService accounts) : Controller
{
    private readonly AccountsService _accounts = accounts;

    public IActionResult Overview()
    {
        var username = HttpContext.Session.GetString("user")!;
        var cuentas = _accounts.GetAccountsFor(username);
        return View(cuentas);
    }
}
