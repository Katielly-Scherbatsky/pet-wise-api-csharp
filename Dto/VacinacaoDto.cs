using System.ComponentModel.DataAnnotations;

namespace Pet.Wise.Api.Dto
{
    public class VacinacaoDto
    {
        [Required(ErrorMessage = "Campo obrigatório")]
        [MinLength(5, ErrorMessage = "Obrigatório mínimo de 5 caracteres")]
        public string NomeVacina { get; set; } = string.Empty;

        [Required(ErrorMessage = "Campo obrigatório")]
        public DateTime DataAplicacao { get; set; }
        public DateTime? DataProximaAplicacao { get; set; }
        public string Observacoes { get; set; } = string.Empty;

        [Required(ErrorMessage = "Campo obrigatório")]
        public int AnimalId { get; set; }

    }
}
