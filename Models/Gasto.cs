using System.ComponentModel.DataAnnotations;

namespace Registro_de_gastos.Models;

/// <summary>
/// Representa un gasto personal registrado por el usuario.
/// </summary>
public class Gasto
{
    public int Id { get; set; }

    [Required(ErrorMessage = "La descripción es obligatoria.")]
    [StringLength(100, ErrorMessage = "La descripción no puede superar los 100 caracteres.")]
    [Display(Name = "Descripción")]
    public string Descripcion { get; set; } = string.Empty;

    [Range(0.01, 100000, ErrorMessage = "El monto debe ser mayor a 0.")]
    [DataType(DataType.Currency)]
    public decimal Monto { get; set; }

    [Display(Name = "Categoría")]
    public CategoriaGasto Categoria { get; set; }

    [DataType(DataType.Date)]
    public DateTime Fecha { get; set; } = DateTime.Today;
}
