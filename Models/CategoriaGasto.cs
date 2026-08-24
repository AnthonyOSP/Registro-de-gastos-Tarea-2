using System.ComponentModel.DataAnnotations;

namespace Registro_de_gastos.Models;

/// <summary>
/// Categorías fijas disponibles para clasificar un gasto.
/// </summary>
public enum CategoriaGasto
{
    [Display(Name = "Alimentación")]
    Alimentacion,

    [Display(Name = "Transporte")]
    Transporte,

    [Display(Name = "Entretenimiento")]
    Entretenimiento,

    [Display(Name = "Salud")]
    Salud,

    [Display(Name = "Educación")]
    Educacion,

    [Display(Name = "Compras")]
    Compras,

    [Display(Name = "Otros")]
    Otros
}
