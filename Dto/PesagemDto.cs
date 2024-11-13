using System.ComponentModel.DataAnnotations;

namespace Pet.Wise.Api.Dto
{
    public class PesagemDto
    {
        [Required(ErrorMessage = "Campo obrigatório")]
        [Range(1, Double.PositiveInfinity,ErrorMessage = "Peso deve ser maior que zero")]
        public decimal Peso { get; set; }

        [Required(ErrorMessage = "Campo obrigatório")]
        public DateTime DataPesagem { get; set; }
        public string Observacoes { get; set; } = string.Empty;

        [Required(ErrorMessage = "Campo obrigatório")]
        public int AnimalId { get; set; }
    }
}
