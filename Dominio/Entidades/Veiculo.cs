using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MinimalApi.Dominio.Entidades;

// ENTIDADE VEICULO
public class Veiculo
{
  [Key]
  [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
  public int Id { get; set; }

  [Required]
  [StringLength(150)]
  public string Nome { get; set; } = default!;

  [StringLength(100)]
  public string Marca { get; set; } = default!;
  
  [StringLength(10)]
  public int Ano { get; set; } = default!;
}
