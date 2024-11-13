using System.ComponentModel.DataAnnotations;

namespace Pet.Wise.Api.Dto
{
    public class LoginDto
    {
        [Required(ErrorMessage = "Campo obrigatório")]
        [EmailAddress(ErrorMessage = "O Campo deve ser um E-mail válido")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Campo obrigatório")]
        [MinLength(8, ErrorMessage = "Obrigatório mínimo de 8 caracteres")]
        public string Senha { get; set; } = string.Empty;
    }
}
