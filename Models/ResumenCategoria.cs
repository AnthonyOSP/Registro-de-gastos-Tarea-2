namespace Registro_de_gastos.Models;

/// <summary>
/// Cuánto se gastó en una categoría, usado en el resumen del dashboard.
/// </summary>
public class ResumenCategoria
{
    public CategoriaGasto Categoria { get; set; }

    public decimal Total { get; set; }

    /// <summary>
    /// Porcentaje que representa esta categoría sobre el total gastado (0-100),
    /// usado para dibujar la barra visual del resumen.
    /// </summary>
    public double Porcentaje { get; set; }
}
