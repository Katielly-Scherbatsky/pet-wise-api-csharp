namespace Pet.Wise.Api.Models
{
    public class PesagemModel : ModelBase
    {
        public decimal Peso { get; set; }
        public DateTime DataPesagem { get; set; }
        public string Observacoes { get; set; } = string.Empty;
        public int AnimalId { get; set; }
    }
}
