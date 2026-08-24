using Microsoft.AspNetCore.Mvc;
using Registro_de_gastos.Models;
using Registro_de_gastos.Services;
using Registro_de_gastos.ViewModels;

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
    // El dashboard (Total, Cantidad, MayorGasto, Promedio, ResumenPorCategoria) se
    // calcula siempre sobre TODOS los gastos; solo la lista de abajo (Gastos) se
    // filtra según lo que el usuario haya escrito en la barra de filtros.
    public IActionResult Index(string? busqueda, CategoriaGasto? categoria, DateTime? fecha)
    {
        var viewModel = new GastosIndexViewModel
        {
            Gastos = _gastoService.FiltrarGastos(busqueda, categoria, fecha),
            Total = _gastoService.ObtenerTotal(),
            Cantidad = _gastoService.ObtenerCantidad(),
            MayorGasto = _gastoService.ObtenerMayorGasto(),
            Promedio = _gastoService.ObtenerPromedio(),
            ResumenPorCategoria = _gastoService.ObtenerTotalesPorCategoria(),
            Busqueda = busqueda,
            Categoria = categoria,
            Fecha = fecha
        };

        return View(viewModel);
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
