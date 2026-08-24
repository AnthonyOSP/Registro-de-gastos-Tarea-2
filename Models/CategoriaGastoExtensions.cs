using System.ComponentModel.DataAnnotations;

namespace Registro_de_gastos.Models;

public static class CategoriaGastoExtensions
{
    /// <summary>
    /// Nombre para mostrar (el texto del atributo [Display]), por ejemplo
    /// "Educación" para CategoriaGasto.Educacion.
    /// </summary>
    public static string ObtenerNombreVisible(this CategoriaGasto categoria)
    {
        var campo = typeof(CategoriaGasto).GetField(categoria.ToString());
        var atributo = campo?.GetCustomAttributes(typeof(DisplayAttribute), false)
            .Cast<DisplayAttribute>()
            .FirstOrDefault();

        return atributo?.Name ?? categoria.ToString();
    }
}
