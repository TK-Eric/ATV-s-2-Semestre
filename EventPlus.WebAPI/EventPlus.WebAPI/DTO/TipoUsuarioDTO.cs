using System.ComponentModel.DataAnnotations;

namespace EventPlus.WebAPI.DTO;

/// <summary>
/// Data Transfer Object (DTO) para cadastro e atualização do Peifl/tipo usuario.
/// </summary>
public class TipoUsuarioDTO
{
    /// <summary>
    /// Titulo do tipo usuario
    /// </summary>
    [Required (ErrorMessage = "O titulo é obrigatorio. ")]
    [StringLength(100, ErrorMessage = "o titulo pode ter no mximo 100 caracteres.")]
    public string Titulo { get; set; } = string.Empty;
}
