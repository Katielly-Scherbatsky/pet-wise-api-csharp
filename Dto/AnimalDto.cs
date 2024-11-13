using System.ComponentModel.DataAnnotations;

namespace Pet.Wise.Api.Dto
{
    public class AnimalDto
    {
        [Required(ErrorMessage = "Campo obrigatório")]
        [MinLength(5, ErrorMessage = "Obrigatório mínimo de 5 caracteres")]
        public string Nome { get; set; } = string.Empty;

        [Required(ErrorMessage = "Campo obrigatório")]
        public DateTime DataNascimento { get; set; }
    }
}
