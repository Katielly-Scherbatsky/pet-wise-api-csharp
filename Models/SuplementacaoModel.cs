using System.ComponentModel.DataAnnotations.Schema;

namespace Pet.Wise.Api.Models
{
    [Table("suplementacao")]
    public class SuplementacaoModel : ModelBase
    {
        [Column("tipo_suplementacao")]
        public string TipoSuplementacao { get; set; } = string.Empty;

        [Column("numero_doses")]
        public int NumeroDoses { get; set; }

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
