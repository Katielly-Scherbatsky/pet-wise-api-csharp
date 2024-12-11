using System.ComponentModel.DataAnnotations;

namespace Pet.Wise.Api.Dto
{
    public class SuplementacaoDto
    {
        [Required(ErrorMessage = "Campo obrigatório")]
        [MinLength(5, ErrorMessage = "Obrigatório mínimo de 5 caracteres")]
        public string TipoSuplementacao { get; set; } = string.Empty;

        [Required(ErrorMessage = "Campo obrigatório")]
        public int NumeroDoses { get; set; }

        [Required(ErrorMessage = "Campo obrigatório")]
        public DateTime DataAplicacao { get; set; }
        public DateTime? DataProximaAplicacao { get; set; }
        public string Observacoes { get; set; } = string.Empty;

        [Required(ErrorMessage = "Campo obrigatório")]
        public int AnimalId { get; set; }

    }
}
