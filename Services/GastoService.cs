using Registro_de_gastos.Models;

namespace Registro_de_gastos.Services;

/// <summary>
/// Administra los gastos en memoria mientras la aplicación está en ejecución.
/// Se registra como singleton para que los datos persistan entre peticiones.
/// </summary>
public class GastoService
{
    private readonly List<Gasto> _gastos = new();
    private int _siguienteId = 1;

    public GastoService()
    {
        CargarDatosDePrueba();
    }

    public List<Gasto> ObtenerTodos()
    {
        return _gastos
            .OrderByDescending(g => g.Fecha)
            .ToList();
    }

    public Gasto? ObtenerPorId(int id)
    {
        return _gastos.FirstOrDefault(g => g.Id == id);
    }

    public Gasto Agregar(Gasto gasto)
    {
        gasto.Id = _siguienteId++;
        _gastos.Add(gasto);
        return gasto;
    }

    public bool Actualizar(Gasto gastoActualizado)
    {
        var gasto = ObtenerPorId(gastoActualizado.Id);
        if (gasto is null)
        {
            return false;
        }

        gasto.Descripcion = gastoActualizado.Descripcion;
        gasto.Monto = gastoActualizado.Monto;
        gasto.Categoria = gastoActualizado.Categoria;
        gasto.Fecha = gastoActualizado.Fecha;
        return true;
    }

    public bool Eliminar(int id)
    {
        var gasto = ObtenerPorId(id);
        if (gasto is null)
        {
            return false;
        }

        return _gastos.Remove(gasto);
    }

    public decimal ObtenerTotal()
    {
        return _gastos.Sum(g => g.Monto);
    }

    public int ObtenerCantidad()
    {
        return _gastos.Count;
    }

    public decimal ObtenerMayorGasto()
    {
        return _gastos.Count == 0 ? 0 : _gastos.Max(g => g.Monto);
    }

    public decimal ObtenerPromedio()
    {
        return _gastos.Count == 0 ? 0 : _gastos.Average(g => g.Monto);
    }

    /// <summary>
    /// Cuánto se gastó en cada categoría (solo las que tienen al menos un gasto),
    /// ordenado de mayor a menor.
    /// </summary>
    public List<ResumenCategoria> ObtenerTotalesPorCategoria()
    {
        var total = ObtenerTotal();

        return _gastos
            .GroupBy(g => g.Categoria)
            .Select(grupo =>
            {
                var totalCategoria = grupo.Sum(g => g.Monto);
                return new ResumenCategoria
                {
                    Categoria = grupo.Key,
                    Total = totalCategoria,
                    Porcentaje = total == 0 ? 0 : (double)(totalCategoria / total * 100)
                };
            })
            .OrderByDescending(r => r.Total)
            .ToList();
    }

    /// <summary>
    /// Filtra los gastos por descripción (contiene, sin importar mayúsculas),
    /// categoría exacta y/o fecha exacta. Cualquier parámetro en null se ignora.
    /// </summary>
    public List<Gasto> FiltrarGastos(string? busqueda, CategoriaGasto? categoria, DateTime? fecha)
    {
        var resultado = ObtenerTodos();

        if (!string.IsNullOrWhiteSpace(busqueda))
        {
            resultado = resultado
                .Where(g => g.Descripcion.Contains(busqueda, StringComparison.OrdinalIgnoreCase))
                .ToList();
        }

        if (categoria.HasValue)
        {
            resultado = resultado.Where(g => g.Categoria == categoria.Value).ToList();
        }

        if (fecha.HasValue)
        {
            resultado = resultado.Where(g => g.Fecha.Date == fecha.Value.Date).ToList();
        }

        return resultado;
    }

    /// <summary>
    /// Gastos de ejemplo para poder probar la aplicación sin necesidad de un formulario todavía.
    /// </summary>
    private void CargarDatosDePrueba()
    {
        Agregar(new Gasto
        {
            Descripcion = "Almuerzo en la universidad",
            Monto = 15.50m,
            Categoria = CategoriaGasto.Alimentacion,
            Fecha = new DateTime(2026, 8, 18)
        });

        Agregar(new Gasto
        {
            Descripcion = "Pasaje de bus",
            Monto = 4.00m,
            Categoria = CategoriaGasto.Transporte,
            Fecha = new DateTime(2026, 8, 19)
        });

        Agregar(new Gasto
        {
            Descripcion = "Entradas al cine",
            Monto = 25.00m,
            Categoria = CategoriaGasto.Entretenimiento,
            Fecha = new DateTime(2026, 8, 20)
        });

        Agregar(new Gasto
        {
            Descripcion = "Libro para el curso",
            Monto = 60.00m,
            Categoria = CategoriaGasto.Educacion,
            Fecha = new DateTime(2026, 8, 21)
        });
    }
}
