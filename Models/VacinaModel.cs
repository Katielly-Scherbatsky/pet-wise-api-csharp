namespace Pet.Wise.Api.Models
{
    public class VacinaModel : ModelBase
    {
        public string NomeVacina { get; set; } = string.Empty;
        public DateTime DataAplicacao { get; set; }
        public DateTime? DataProximaAplicacao { get; set; }
        public string Observacoes { get; set; } = string.Empty;
        public int AnimalId { get; set; }
    }
}
