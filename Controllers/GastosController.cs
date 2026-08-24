using Microsoft.AspNetCore.Mvc;
using Registro_de_gastos.Models;
using Registro_de_gastos.Services;

namespace Registro_de_gastos.Controllers;

/// <summary>
/// CRUD de gastos. Toda la lógica de datos vive en <see cref="GastoService"/>;
/// este controlador solo recibe la petición, valida y decide qué vista mostrar.
/// </summary>
public class GastosController : Controller
{
    private readonly GastoService _gastoService;

    public GastosController(GastoService gastoService)
    {
        _gastoService = gastoService;
    }

    // GET: Gastos
    public IActionResult Index()
    {
        var gastos = _gastoService.ObtenerTodos();
        ViewBag.Total = _gastoService.ObtenerTotal();
        return View(gastos);
    }

    // GET: Gastos/Details/5
    public IActionResult Details(int id)
    {
        var gasto = _gastoService.ObtenerPorId(id);
        if (gasto is null)
        {
            return NotFound();
        }

        return View(gasto);
    }

    // GET: Gastos/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: Gastos/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(Gasto gasto)
    {
        if (!ModelState.IsValid)
        {
            return View(gasto);
        }

        _gastoService.Agregar(gasto);
        return RedirectToAction(nameof(Index));
    }

    // GET: Gastos/Edit/5
    public IActionResult Edit(int id)
    {
        var gasto = _gastoService.ObtenerPorId(id);
        if (gasto is null)
        {
            return NotFound();
        }

        return View(gasto);
    }

    // POST: Gastos/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(int id, Gasto gasto)
    {
        if (id != gasto.Id)
        {
            return NotFound();
        }

        if (!ModelState.IsValid)
        {
            return View(gasto);
        }

        var actualizado = _gastoService.Actualizar(gasto);
        if (!actualizado)
        {
            return NotFound();
        }

        return RedirectToAction(nameof(Index));
    }

    // GET: Gastos/Delete/5
    public IActionResult Delete(int id)
    {
        var gasto = _gastoService.ObtenerPorId(id);
        if (gasto is null)
        {
            return NotFound();
        }

        return View(gasto);
    }

    // POST: Gastos/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public IActionResult DeleteConfirmed(int id)
    {
        var eliminado = _gastoService.Eliminar(id);
        if (!eliminado)
        {
            return NotFound();
        }

        return RedirectToAction(nameof(Index));
    }
}
