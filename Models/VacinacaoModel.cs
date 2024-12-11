using System.ComponentModel.DataAnnotations.Schema;

namespace Pet.Wise.Api.Models
{
    [Table("vacinacao")]
    public class VacinacaoModel : ModelBase
    {
        [Column("nome_vacina")]
        public string NomeVacina { get; set; } = string.Empty;

        [Column("data_aplicacao")]
        public DateTime DataAplicacao { get; set; }

        [Column("data_proxima_aplicacao")]
        public DateTime? DataProximaAplicacao { get; set; }

        [Column("observacoes")]
        public string Observacoes { get; set; } = string.Empty;

        [Column("animal_id")]
        public int AnimalId { get; set; }
    }
}
