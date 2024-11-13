using System.ComponentModel.DataAnnotations;

namespace Pet.Wise.Api.Dto
{
    public class UsuarioDto
    {
        [Required(ErrorMessage = "Campo obrigatório")]
        [MinLength(5, ErrorMessage = "Obrigatório mínimo de 5 caracteres")]
        public string Nome { get; set; } = string.Empty;

        [Required(ErrorMessage = "Campo obrigatório")]
        [EmailAddress(ErrorMessage = "O Campo deve ser um E-mail válido")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Campo obrigatório")]
        [MinLength(8, ErrorMessage = "Obrigatório mínimo de 8 caracteres")]
        public string Senha { get; set; } = string.Empty;
    }
}
