
using Microsoft.AspNetCore.Mvc;
using BancaEnLinea.Services;
using BancaEnLinea.Models;

namespace BancaEnLinea.Controllers;

public class BeneficiariosController(BeneficiariosService service) : Controller
{
    private readonly BeneficiariosService _service = service;

    public IActionResult Index()
    {
        var username = HttpContext.Session.GetString("user")!;
        return View(_service.List(username));
    }

    [HttpGet]
    public IActionResult Create() => View();

    [HttpPost]
    public IActionResult Create(string alias, string banco, string numeroCuenta)
    {
        var username = HttpContext.Session.GetString("user")!;
        var (ok, error, b) = _service.Create(username, alias, banco, numeroCuenta);
        if (!ok)
        {
            ViewBag.Error = error;
            return View();
        }
        TempData["msg"] = "Beneficiario agregado correctamente";
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public IActionResult Edit(string id)
    {
        var username = HttpContext.Session.GetString("user")!;
        var b = _service.Get(id, username);
        if (b is null) return NotFound();
        return View(b);
    }

    [HttpPost]
    public IActionResult Edit(string id, string alias, string banco, string numeroCuenta)
    {
        var username = HttpContext.Session.GetString("user")!;
        var (ok, error) = _service.Update(id, username, alias, banco, numeroCuenta);
        if (!ok)
        {
            ViewBag.Error = error;
            var b = _service.Get(id, username) ?? new Beneficiario{ Id = id, Alias = alias, Banco = banco, NumeroCuenta = numeroCuenta };
            return View(b);
        }
        TempData["msg"] = "Beneficiario actualizado";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    public IActionResult Delete(string id)
    {
        var username = HttpContext.Session.GetString("user")!;
        _service.Delete(id, username);
        TempData["msg"] = "Beneficiario eliminado";
        return RedirectToAction(nameof(Index));
    }
}
