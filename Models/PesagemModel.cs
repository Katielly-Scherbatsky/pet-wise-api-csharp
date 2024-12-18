using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Pet.Wise.Api.Models
{
    [Table("pesagem")]
    public class PesagemModel : ModelBase
    {
        [Column("peso")]
        public decimal Peso { get; set; }

        [Column("data_pesagem")]
        public DateTime DataPesagem { get; set; }

        [Column("observacoes")]
        public string Observacoes { get; set; } = string.Empty;

        [Column("animal_id")]
        public int AnimalId { get; set; }

        public virtual AnimalModel Animal { get; set; }
    }
}
