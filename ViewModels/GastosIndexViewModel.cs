using Microsoft.AspNetCore.Mvc.Rendering;
using Registro_de_gastos.Models;

namespace Registro_de_gastos.ViewModels;

/// <summary>
/// Todo lo que necesita la vista Index de Gastos: el dashboard (calculado sobre
/// TODOS los gastos), el resumen por categoría y la lista ya filtrada, junto con
/// los valores de filtro actuales para repoblar el formulario y armar los enlaces.
/// </summary>
public class GastosIndexViewModel
{
    // Lista a mostrar en la tabla, ya filtrada según Busqueda/Categoria/Fecha.
    public List<Gasto> Gastos { get; set; } = new();

    // Estadísticas del dashboard: se calculan sobre todos los gastos, sin filtrar,
    // para que las tarjetas de resumen no cambien solo porque se aplicó un filtro.
    public decimal Total { get; set; }
    public int Cantidad { get; set; }
    public decimal MayorGasto { get; set; }
    public decimal Promedio { get; set; }
    public List<ResumenCategoria> ResumenPorCategoria { get; set; } = new();

    // Valores de filtro actuales, para repoblar el formulario.
    public string? Busqueda { get; set; }
    public CategoriaGasto? Categoria { get; set; }
    public DateTime? Fecha { get; set; }

    public bool HayFiltrosActivos =>
        !string.IsNullOrWhiteSpace(Busqueda) || Categoria.HasValue || Fecha.HasValue;

    // Si no hay ningún gasto registrado (independientemente de los filtros).
    public bool HayGastosRegistrados => Cantidad > 0;

    // Opciones para el <select> de categoría del formulario de filtros. Value es
    // el nombre del enum (p. ej. "Transporte"), no el número, para que la URL del
    // filtro (?categoria=Transporte) sea legible y la opción marcada como
    // seleccionada coincida siempre con Categoria.
    public List<SelectListItem> OpcionesCategoria { get; } =
        Enum.GetValues<CategoriaGasto>()
            .Select(c => new SelectListItem(c.ObtenerNombreVisible(), c.ToString()))
            .ToList();
}
