
using Microsoft.AspNetCore.Mvc;
using BancaEnLinea.Services;

namespace BancaEnLinea.Controllers;

public class HistorialController(HistorialService historial) : Controller
{
    private readonly HistorialService _historial = historial;

    [HttpGet]
    public IActionResult Index(DateTime? desde, DateTime? hasta, string? tipo)
    {
        var username = HttpContext.Session.GetString("user")!;
        var items = _historial.Filtrar(username, desde, hasta, tipo);
        return View(items);
    }

    [HttpGet]
    public IActionResult Export(DateTime? desde, DateTime? hasta, string? tipo)
    {
        var username = HttpContext.Session.GetString("user")!;
        var items = _historial.Filtrar(username, desde, hasta, tipo);
        var (data, name) = _historial.ExportarCsv(items);
        return File(data, "text/csv", name);
    }
}
