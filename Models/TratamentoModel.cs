using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Pet.Wise.Api.Models
{
    [Table("tratamento")]
    public class TratamentoModel : ModelBase
    {
        [Column("tipo_tratamento")]
        public string TipoTratamento { get; set; } = string.Empty;

        [Column("data_aplicacao")]
        public DateTime DataAplicacao { get; set; }

        [Column("data_proxima_aplicacao")]
        public DateTime? DataProximaAplicacao { get; set; }

        [Column("observacoes")]
        public string Observacoes { get; set; } = string.Empty;

        [Column("animal_id")]
        public int AnimalId { get; set; }

        public virtual AnimalModel Animal { get; set; }
    }
}
