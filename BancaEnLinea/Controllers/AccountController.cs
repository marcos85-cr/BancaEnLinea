
using Microsoft.AspNetCore.Mvc;
using BancaEnLinea.Services;

namespace BancaEnLinea.Controllers;

public class AccountController(AuthService auth) : Controller
{
    private readonly AuthService _auth = auth;

    [HttpGet]
    public IActionResult Login(string? returnUrl = null)
    {
        ViewBag.ReturnUrl = returnUrl;
        return View();
    }

    [HttpPost]
    public IActionResult Login(string username, string password, string? returnUrl = null)
    {
        var u = _auth.Login(username, password);
        if (u is null)
        {
            ViewBag.Error = "Credenciales inválidas";
            return View();
        }
        HttpContext.Session.SetString("user", u.Username);
        HttpContext.Session.SetString("fullName", u.FullName);
        return Redirect(string.IsNullOrWhiteSpace(returnUrl) ? "/Dashboard" : returnUrl!);
    }

    public IActionResult Logout()
    {
        HttpContext.Session.Clear();
        return RedirectToAction(nameof(Login));
    }
}
